---
name: RD Gateway 与重连策略
about: 支持 RD Gateway 配置及可配置的连接重试/重连策略
labels: enhancement, area:connection
---

## 描述
实现 RD Gateway 参数（地址、端口、凭据使用模式、证书验证选项）并在连接中断时依据用户配置进行重连。

## 任务
- [ ] 在 `Profile` 模型中添加 `Gateway` 子结构（地址、端口、useCredentialType、validateCertificate）
- [ ] 在 UI 中增加网关配置输入项
- [ ] 在连接逻辑中尊重重连参数（最大重试次数、间隔、后台自动重连）
- [ ] 在重连过程中记录每次尝试的结果用于诊断

## 验收标准
能通过 UI 配置网关并使用它建立连接；在连接失败时按配置进行重连并在 UI/日志中反映出来。