---
name: JSON 配置 schema 更新（包含 Raw .rdp 字段）
about: 将高级 `.rdp` 字段加入到项目的配置导入/导出 schema
labels: enhancement, area:data
---

## 描述
更新 JSON schema，使导出的 JSON 配置包含 `RawRdp` 字段和网关/重连配置子字段。

## 任务
- [ ] 定义 `RawRdp` 字段及其格式说明
- [ ] 在导入/导出逻辑中包含这些字段的读写
- [ ] 更新示例配置与文档

## 验收标准
导出的 JSON 包含新字段并可被重新导入还原同样配置。