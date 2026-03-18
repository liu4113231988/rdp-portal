---
name: 连接稳定性与选项
about: 跟踪“连接稳定性与选项”功能的实现（高级 .rdp 参数、RD Gateway、重连策略、日志等）
labels: enhancement, area:connection
---

## 背景
需要在 UI 中保存并编辑完整的 `.rdp` 参数，支持 RD Gateway 设置，并提供可配置的重连策略与诊断日志。

## 目标
- 在配置中包含完整 `.rdp` 字段
- 支持 RD Gateway 参数与凭据配置
- 提供重连策略与日志导出

## 任务清单
- [ ] 在 UI 中增加“高级 RDP 参数”编辑区域（原始 `.rdp` 文本或键值对视图）
- [ ] 导入/导出 `.rdp` 文件并预览生效参数
- [ ] 将字段序列化到 JSON 配置中
- [ ] 支持 RD Gateway（地址/端口/凭据/证书验证）
- [ ] 实现重连策略（重试次数/间隔/后台自动重连）
- [ ] 诊断日志记录与导出
- [ ] 使用安全存储保存凭据（Windows Credential Manager / DPAPI）

## 验收标准
见 `todo.md` 中“连接稳定性与选项”节。

## 实现建议
- 在 `Profile` 模型中增加 `RawRdp` 字段用于保存原始 `.rdp` 文本
- 在 `RdpFile` 中添加序列化/反序列化支持
- 在 `MainForm` 增加一个折叠面板或 Tab，用于编辑/导入/导出 `.rdp` 文本
- 将凭据存储迁移至 Windows Credential Manager

## 附注
请在实现 PR 中引用该 issue。