# TODO - RDP Portal 功能改进

## Bug 修复
- [x] 修复 `Profile.cs` 文件开头的重复 BOM 字符（与之前 .csproj 相同的问题）
- [x] 修复 `RdpFile.cs` 文件开头的重复 BOM 字符
- [x] 修复 `MainForm.cs` 中 `lastSelectedNode` 字段未使用的警告
- [x] 修复 nullable 引用类型警告（CS8625, CS8602, CS8600, CS8603, CS8618, CS8604）
- [x] 修复 `SelectProfile` 方法中选中空白区域时按钮可见性不一致的问题

## 功能增强


### Profile 管理
- [x] 添加 Profile 复制/克隆功能
- [x] 支持拖拽 Profile 到不同 Group

### 连接功能
- [x] 添加连接前主机可达性检测（ping）

### 界面改进
- [x] 添加 TreeView 节点图标（Group/Profile 区分）
- [x] 添加快捷键支持（Ctrl+N 新建, Ctrl+S 保存, F5 连接等）
- [ ] 添加连接状态指示器
- [x] 支持窗口大小和位置记忆

### 数据管理
- [ ] 添加完整备份/恢复功能（包含数据库和 RDP 文件）
- [x] 支持导出为 CSV 格式
- [x] 添加数据验证（导入时检查重复项）
- [x] 支持 Group 的导入导出

### 其他
- [ ] 添加操作日志查看界面
- [x] 改进错误提示的用户体验
