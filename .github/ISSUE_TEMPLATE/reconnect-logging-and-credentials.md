---
name: 重连日志与凭据安全存储
about: 记录重连/连接失败日志并确保凭据使用安全存储
labels: enhancement, security
---

## 描述
可选开启的诊断日志记录连接与重连过程，并能导出；凭据需要使用 Windows Credential Manager 或 DPAPI 存储。

## 任务
- [ ] 增加诊断日志开关并实现本地日志写入
- [ ] 提供 UI 入口查看/导出日志
- [ ] 集成 Windows Credential Manager（或使用 DPAPI）保存凭据并在导出时排除凭据明文

## 验收标准
诊断日志能导出作为文本文件；导出配置不包含明文凭据。