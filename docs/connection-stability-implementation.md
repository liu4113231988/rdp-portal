# 连接稳定性与选项 — 详细实现方案

## 总体目标
- 将高级 `.rdp` 参数（原始文本）纳入配置模型并在 UI 中可视化编辑。
- 支持 RD Gateway 配置、可配置的重连策略、诊断日志与凭据安全存储。

## 分阶段计划（优先顺序）
1. **基础：Raw .rdp 编辑**（已实现最小版本）
   - 在 `Profile` 添加 `RawRdp` 字段；在 UI 增加多行文本框用于编辑/导入/导出 `.rdp`。
   - 行为：如 `RawRdp` 非空，则写入 `.rdp` 文件；否则保留现有自动生成逻辑。
2. **JSON schema 与导入导出**
   - 更新导出/导入逻辑，确保 `RawRdp`、网关、重连字段纳入 JSON。
3. **RD Gateway 支持**
   - 在模型中增加 `Gateway` 子对象，UI 增加输入项。
4. **重连策略与诊断日志**
   - 实现可配置的重连策略并在连接/失败时记录日志，提供日志导出。
5. **凭据安全存储**
   - 集成 Windows Credential Manager 或使用 DPAPI 保存凭据，导出时不包含明文。
6. **增强 UX**
   - 增加参数验证、键值对编辑器、参数预览/合并视图。

## 逐文件修改清单（建议）

- `src/Profile.cs`
  - 新增 `public string RawRdp { get; set; }`（已添加）。
  - 在 `PrepareRdpFile()` 中：如果 `RawRdp` 非空则直接写入 `.rdp` 文件（已实现）。
  - 将网关、重连等额外字段加入模型（后续）。

- `src/RdpFile.cs`
  - 可作为工具类：解析 `.rdp` 文本为键值对、合并/覆盖常见字段、生成安全导出（移除密码）。
  - 函数建议：`ParseRaw(string) -> IDictionary<string,string>`，`Serialize(IDictionary) -> string`。

- `src/MainForm.Designer.cs` / `src/MainForm.cs`
  - 在右侧详情区域增加 `groupBoxAdvanced`，包含多行 `textBoxRawRdp`、`Import/Export/Preview` 按钮（最小实现已添加）。
  - 在 `SelectProfile()` 加载 `profile.RawRdp`，在 `buttonSave_Click` 保存回 `profile.RawRdp`（已添加）。
  - 添加 `buttonImportRdp_Click`、`buttonExportRdp_Click`、`buttonPreviewRdp_Click` 事件处理器（已实现）。

- `src/Config.cs`
  - 更新导入/导出逻辑以包含新增字段，保持向后兼容。

- 凭据存储相关
  - 新增一个 `CredentialsStore` helper，封装 Windows Credential Manager 或 DPAPI 的读写接口。
  - 修改 `Profile` 中对密码的处理，尽量移除将明文写入 JSON 的情况，默认仅保存加密版本或凭据引用。

- 日志与诊断
  - 新增 `Logs` 目录与 `Logger` 简单实现，支持按 profile 记录连接/重连事件并导出为文本。
  - 在 UI 中增加“查看日志/导出日志”按钮与对话框。

## 验收测试建议
- 手动用以下场景验证：
  - 从 `.rdp` 文件导入并保存为配置，再导出 JSON，确认 `RawRdp` 字段存在。
  - 通过 UI 修改 `RawRdp` 并连接（mstsc），确认生成的 .rdp 文件内容匹配。
  - 配置 RD Gateway（后续）并验证能成功通过网关连接。
  - 在连接失败时触发重连并检查日志记录。

## 安全与隐私注意事项
- 导出 JSON 时默认不包含明文密码或凭据，除非用户明确选择“包含凭据导出”。
- 使用 Windows Credential Manager 或 DPAPI 时要记录兼容平台（仅 Windows）。

## 后续 PR 建议拆分
- PR 1：RawRdp UI + Profile.RawRdp 支持 + 基本导入/导出（已实现）
- PR 2：RdpFile 解析/序列化工具 + JSON schema 更新
- PR 3：RD Gateway UI + 模型 + 连接逻辑改造
- PR 4：重连策略实现 + 日志记录 + 日志 UI
- PR 5：凭据安全存储集成与文档

---

如果你同意，我可以基于上面的拆分依次提交更小的 PR（我可以先把 PR 草案放到 `draft/` 分支并在本地生成变更）。