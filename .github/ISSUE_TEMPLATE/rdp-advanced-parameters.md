---
name: 高级 RDP 参数（Raw .rdp 编辑）
about: 在 UI 中增加可编辑的 `.rdp` 原始文本区域，并支持导入/导出/预览
labels: enhancement, area:ui
---

## 描述
添加一个多行文本编辑框用于显示和编辑原始 `.rdp` 内容，提供导入 `.rdp`、导出 `.rdp`、预览生效参数的功能。

## 任务
- [ ] 在 `MainForm` 中增加 UI（GroupBox 或 Tab）包含：多行 `TextBox`、`Import`、`Export`、`Preview` 按钮
- [ ] 在 `Profile`/`RdpFile` 中增加 `RawRdp` 字段并支持序列化
- [ ] 导入时解析常见字段并在 UI 中以键值对或原始文本显示
- [ ] 导出时确保凭据不以明文写入导出的 JSON（除非用户明确要求）

## 验收标准
- 能从 `.rdp` 文件导入并在 UI 中编辑后保存回配置
- 能导出当前 `.rdp` 文本为文件

## 实现建议
优先实现原始文本编辑与导入/导出，后续可加键值对编辑器和参数验证。