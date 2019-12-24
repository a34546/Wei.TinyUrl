# Wei.TinyUrl
基于.NetCore3.0 + Mysql开发的短网址项目

## 快速开始
**1. 修改连接字符串**
- appsettings.Development.json中ConnectionStrings修改为自己的mysql连接
	
**2. 更新数据库**
- 打开程序包管理器控制台
- 选中Wei.TinyUrl.Data项目，执行 Update-Database命令
	
**3. 启动项目**
- 设置Wei.TinyUrl.Api为启动项目
- F5运行项目

**4. 生成短链接**
- 浏览器输入 http://localhost:5000/api/create?url=https://github.com/a34546/Wei.TinyUrl.git&key=123456789 
- 访问生成的短链接
