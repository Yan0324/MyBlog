# MyBlog 部署文档

## 前期准备（部署前必读）

在开始部署之前，需要完成以下准备事项。

### 一、选择部署路线

根据你的需求和预算，有以下两种路线可选：

| 对比维度 | 路线 A：国内云 + 备案 | 路线 B：海外云（免备案） |
|---|---|---|
| 服务器厂商 | 阿里云、腾讯云、华为云 | AWS、Vultr、BandwagonHost、CloudCone |
| 域名注册 | 阿里云万网、腾讯云 DNSPod | Namecheap、Cloudflare、GoDaddy |
| 备案要求 | **必须**完成 ICP 备案 | 无需备案 |
| 国内访问速度 | 快（延迟 < 50ms） | 较慢（延迟 100-300ms） |
| 价格 | 新用户约 ¥60-100/年 | 约 $3-6/月（¥20-45/月） |
| 部署后即可访问 | 否，需等备案（约 15-20 个工作日） | 是，买完即可访问 |

> 如果你希望国内用户快速访问且不介意备案流程，选路线 A；如果追求快速上线、免备案，选路线 B。

---

### 二、路线 A：国内云 + 备案（详细步骤）

#### 2.1 购买云服务器（ECS / CVM / 轻量应用服务器）

**推荐配置（个人博客完全够用）：**

| 配置项 | 建议 |
|---|---|
| CPU | 1-2 核 |
| 内存 | 2 GB |
| 系统盘 | 40-60 GB SSD |
| 带宽 | 3-5 Mbps（按量计费或固定带宽） |
| 操作系统 | Ubuntu 22.04 LTS 或 Ubuntu 24.04 LTS |
| 地域 | 选择离你和目标用户最近的地域（如华东、华南） |

**主流厂商及入口：**

| 厂商 | 产品名称 | 网址 |
|---|---|---|
| 阿里云 | ECS 云服务器 / 轻量应用服务器 | https://www.aliyun.com |
| 腾讯云 | CVM 云服务器 / 轻量应用服务器 | https://cloud.tencent.com |
| 华为云 | ECS 弹性云服务器 | https://www.huaweicloud.com |

> **省钱技巧**：各大云厂商对新用户有大幅折扣，通常 ¥60-100/年即可买到 2核2G 配置。建议先买 1 年，到期续费会涨价，届时可换厂商重新享受新用户价。

**购买时关键步骤：**

1. 选择「公共镜像」→ Ubuntu 22.04 / 24.04
2. 设置 root 密码（务必牢记，后续 SSH 登录用）
3. 安全组（防火墙）先开放 22（SSH）、80（HTTP）、443（HTTPS）端口
4. 购买后会获得 **公网 IP**（如 `47.xx.xx.xx`），记下来

#### 2.2 注册域名

**域名注册平台：**

| 平台 | 网址 | 特点 |
|---|---|---|
| 阿里云（万网） | https://wanwang.aliyun.com | 国内最大，与阿里云 ECS 配套方便 |
| 腾讯云（DNSPod） | https://dnspod.cloud.tencent.com | 与腾讯云 CVM 配套方便 |
| 华为云 | https://www.huaweicloud.com | 与华为云 ECS 配套方便 |

**选域名建议：**

- 首选 `.com`，备选 `.cn`、`.top`、`.blog`
- 简短、好记、与你博客主题相关
- 价格：`.com` 约 ¥60-80/年，`.cn` 约 ¥30/年，`.top` 首年几块钱
- 避免使用生僻拼音和过长单词组合

#### 2.3 域名实名认证（必须）

购买域名后，根据工信部要求，必须完成**域名实名认证**，否则域名会被暂停解析。

1. 登录域名注册商控制台
2. 找到「域名列表」→ 对应域名 → 「实名认证」
3. 上传身份证正反面照片
4. 等待审核（通常 1-3 个工作日）

#### 2.4 ICP 备案（必须在域名实名认证通过后操作）

> **重要**：使用国内云服务器 + 国内域名，ICP 备案是强制要求。没有备案号，无法通过域名访问网站。备案**免费**。

**备案流程：**

1. 登录云厂商控制台，找到「备案」入口（阿里云叫「ICP 备案」，腾讯云叫「网站备案」）
2. 填写备案信息：
   - 主体信息：个人姓名、身份证号、住址、联系方式
   - 网站信息：网站名称（不能包含"中国""中华"等敏感词，个人博客建议用"XX 的个人博客"等）、域名、网站简介
3. 上传材料：
   - 身份证正反面照片
   - **人脸核身**（手机扫码完成人脸识别）
   - 部分地区可能需要**手持身份证照片**或**域名证书**
4. 提交后等待：
   - 云厂商初审：1-2 个工作日
   - 工信部短信核验：收到短信后 24 小时内访问 https://beian.miit.gov.cn 完成核验
   - 管局终审：**10-15 个工作日**（不同省份时间不同）
5. 备案通过后，获取 **ICP 备案号**（如「京 ICP 备 xxxxxx 号」）

**备案期间可以做什么：**

备案审核期间域名无法访问，但你可以通过**公网 IP 直接访问**服务器进行部署和测试：

```
http://你的公网IP
```

所以备案等待期间，可以先完成后续所有部署步骤，备案通过后立即可通过域名访问。

#### 2.5 公安联网备案（部分省份要求）

ICP 备案通过后，部分地区（如北京、上海、广东）要求在 30 天内完成**公安联网备案**：

1. 访问 https://www.beian.gov.cn
2. 注册账号 → 填写网站信息 → 提交
3. 审核通过后将备案号放在网站底部

> 此项各地执行力度不同，先完成 ICP 备案即可上线，公安备案可后续补办。

---

### 三、路线 B：海外云（免备案）

如果你不想等待 ICP 备案，可以选择海外服务器。

**推荐厂商：**

| 厂商 | 最低价格 | 特点 |
|---|---|---|
| Vultr | $6/月 | 全球多机房，按小时计费，随时销毁 |
| BandwagonHost（搬瓦工） | $49/年 | 性价比高，CN2 GIA 线路对国内较快 |
| CloudCone | $3/月 | 便宜，美国洛杉矶机房，支持支付宝 |
| AWS Lightsail | $3.5/月起 | 大厂稳定，前 3 个月免费 |
| RackNerd | $12/年起 | 极致便宜，黑五促销价更低 |

**域名注册（推荐海外平台）：**

| 平台 | 特点 |
|---|---|
| Namecheap | 支持支付宝，提供免费 Whois 隐私保护 |
| Cloudflare Registrar | **成本价**（无加价），免费 CDN + DDoS 防护 |
| Porkbun | 价格低，赠送免费 SSL |

**海外路线优势：**

- 无需备案，买完服务器和域名后 **10 分钟内即可上线**
- 无内容审核限制（但仍需遵守服务器所在地法律）
- 域名无需实名认证

**海外路线劣势：**

- 国内访问速度偏慢（可通过 Cloudflare CDN 改善）
- 部分廉价 VPS 稳定性一般

---

### 四、域名解析（DNS 配置）

无论选择哪条路线，最终都要将域名指向服务器公网 IP。

1. 登录域名 DNS 管理后台（在哪买的域名就去哪管）
2. 添加 A 记录：

| 记录类型 | 主机记录 | 记录值 |
|---|---|---|
| A | `@`（代表根域名） | 你的公网 IP |
| A | `www` | 你的公网 IP |

3. 保存后等待 DNS 生效（通常几分钟到几小时）

验证是否生效：

```bash
ping 你的域名.com
# 应该返回你的服务器公网 IP
```

---

### 五、费用预估

#### 路线 A（国内云 + 备案）

| 项目 | 费用（首年） |
|---|---|
| 云服务器（2核2G） | ¥60-100（新用户优惠价） |
| 域名（.com） | ¥60-80 |
| ICP 备案 | 免费 |
| SSL 证书 | 免费（Let's Encrypt） |
| **合计** | **约 ¥120-180** |

> 次年服务器续费会恢复原价（约 ¥500-800/年），届时可更换云厂商按新用户价重新购买。

#### 路线 B（海外云）

| 项目 | 费用（首年） |
|---|---|
| VPS（1核1G） | $12-36（¥85-260） |
| 域名（.com） | $9-12（¥65-85） |
| SSL 证书 | 免费（Let's Encrypt） |
| **合计** | **约 ¥150-350** |

---

### 六、准备清单

部署前，请确认以下信息已准备就绪：

| 事项 | 内容 | 状态 |
|---|---|---|
| 服务器公网 IP | `____.____.____.____` | ☐ |
| SSH 用户名 | `root`（默认） | ☐ |
| SSH 密码 / 密钥 | `________` | ☐ |
| 服务器系统 | Ubuntu 22.04 / 24.04 | ☐ |
| 域名 | `________.com` | ☐ |
| 域名已实名认证 | ✅ / 不需要 | ☐ |
| ICP 备案号 | `________` / 不需要 | ☐ |
| DNS 解析已生效 | ping 域名返回服务器 IP | ☐ |
| 安全组 / 防火墙已开放 22, 80, 443 | ✅ | ☐ |

> 以上信息全部确认后，即可进入下方的部署操作。

---

## 项目架构

```
浏览器
  │
  ▼
https://你的域名.com (443)
  │
  ▼
┌─────────────┐
│   Nginx     │
│             │
│  /          ├──► /var/www/myblog/html/  (Vue 3 SPA 静态文件)
│  /api/*     ├──► http://localhost:5000  (ASP.NET Core Web API)
└─────────────┘
                    │
                    ▼
              ┌──────────┐
              │  MySQL   │
              │  8.0+    │
              └──────────┘
```

| 组件 | 技术栈 |
|---|---|
| 前端 | Vue 3 + Vue CLI 5，单页应用（SPA），构建输出 `web_page/dist/` |
| 后端 | ASP.NET Core Web API (.NET 10)，监听 `http://localhost:5000` |
| 数据库 | MySQL 8.0+，EF Core 9.0 + Pomelo MySQL Provider |
| API 前缀 | 全部接口以 `/api` 开头 |
| 反向代理 | Nginx，同域名下服务前端静态文件 + 代理 API 请求 |

---

---

## 正式部署

> 以下步骤假设你已完成「前期准备」中的所有事项，即：拥有服务器、公网 IP 已开放端口、域名已解析到服务器。

### 第一步：服务器环境准备

适用系统：Ubuntu 24.04 / Debian 12。其他发行版请自行调整包管理器命令。

**通过 SSH 连接到服务器：**

```bash
ssh root@你的公网IP
```

登录后执行以下操作：

### 1.1 安装 .NET 10 SDK

```bash
wget https://packages.microsoft.com/config/ubuntu/24.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt update
sudo apt install -y dotnet-sdk-10.0

# 验证安装
dotnet --version
```

### 1.2 安装 Node.js 20 LTS

```bash
curl -fsSL https://deb.nodesource.com/setup_20.x | sudo -E bash -
sudo apt install -y nodejs

# 验证安装
node --version
npm --version
```

### 1.3 安装 MySQL 8.0

```bash
sudo apt install -y mysql-server
sudo systemctl enable mysql
sudo systemctl start mysql
```

### 1.4 安装 Nginx

```bash
sudo apt install -y nginx
sudo systemctl enable nginx
sudo systemctl start nginx
```

---

## 第二步：MySQL 数据库配置

### 2.1 安全初始化

```bash
sudo mysql_secure_installation
```

### 2.2 创建数据库和用户

```bash
sudo mysql -u root
```

在 MySQL 命令行中执行：

```sql
CREATE DATABASE blog CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
CREATE USER 'blog_user'@'localhost' IDENTIFIED BY '替换为你的强密码';
GRANT ALL PRIVILEGES ON blog.* TO 'blog_user'@'localhost';
FLUSH PRIVILEGES;
EXIT;
```

---

## 第三步：上传项目代码

```bash
# 创建项目目录
sudo mkdir -p /var/www/myblog
sudo chown $USER:$USER /var/www/myblog

# 方式一：Git clone（推荐，方便后续更新）
cd /var/www/myblog
git clone <你的仓库地址> .

# 方式二：SCP 上传（在本地 Windows 终端执行）
# scp -r D:\learning\MyBlog\MyBlog\* user@服务器IP:/var/www/myblog/
```

---

## 第四步：生产环境配置

### 4.1 后端配置文件

创建 `blog_server/blog_server/appsettings.Production.json`：

```bash
nano /var/www/myblog/blog_server/blog_server/appsettings.Production.json
```

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=blog;User=blog_user;Password=你设置的数据库密码;CharSet=utf8mb4;"
  },
  "Jwt": {
    "SecretKey": "替换为随机密钥（至少 32 字符）",
    "Issuer": "MyBlog",
    "Audience": "MyBlog",
    "ExpirationMinutes": 480
  },
  "Admin": {
    "PasswordHash": "替换为 BCrypt 哈希（见下方说明）"
  }
}
```

> **安全提醒**：
> - `Jwt:SecretKey` 用于签发和验证 JWT，**至少 32 字符**。可以用 `openssl rand -hex 32` 生成。
> - `Admin:PasswordHash` 是 BCrypt 哈希，不要填写明文密码。生成方式：在后端代码中调用 `BCrypt.Net.BCrypt.HashPassword("你的密码")` 获取哈希值。
> - 也支持 `Admin:Password`（明文）作为开发过渡配置，后端启动时会自动哈希；生产环境请使用 `PasswordHash`。

### 4.2 前端环境变量

确认 `web_page/.env.production` 内容为：

```
VUE_APP_API_BASE=/api
```

> 前后端同域名部署，使用相对路径 `/api` 即可，无需修改。

---

## 第五步：构建并部署后端

### 5.1 还原依赖并执行数据库迁移

```bash
cd /var/www/myblog/blog_server

# 还原 NuGet 包
dotnet restore

# 安装 EF Core 命令行工具
dotnet tool install --global dotnet-ef
export PATH="$PATH:$HOME/.dotnet/tools"

# 执行数据库迁移（创建表结构）
dotnet ef database update --project blog_server/blog_server.csproj
```

### 5.2 发布项目

```bash
dotnet publish blog_server/blog_server.csproj \
    --configuration Release \
    --runtime linux-x64 \
    --self-contained false \
    --output /var/www/myblog/publish
```

### 5.3 注册 systemd 服务

```bash
sudo nano /etc/systemd/system/myblog-api.service
```

```ini
[Unit]
Description=MyBlog ASP.NET Core API
After=network.target mysql.service

[Service]
WorkingDirectory=/var/www/myblog/publish
ExecStart=/usr/bin/dotnet /var/www/myblog/publish/blog_server.dll
Restart=always
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=myblog-api
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=ASPNETCORE_URLS=http://localhost:5000

[Install]
WantedBy=multi-user.target
```

启动服务：

```bash
sudo systemctl daemon-reload
sudo systemctl enable myblog-api
sudo systemctl start myblog-api

# 检查状态
sudo systemctl status myblog-api

# 验证 API 响应
curl http://localhost:5000/api/status
```

### 5.4 查看日志

```bash
# 实时日志
sudo journalctl -u myblog-api -f

# 最近日志
sudo journalctl -u myblog-api -n 50 --no-pager
```

---

## 第六步：构建并部署前端

```bash
cd /var/www/myblog/web_page

# 安装依赖
npm install

# 生产构建
npm run build

# 部署到 nginx 目录
sudo mkdir -p /var/www/myblog/html
sudo cp -r dist/* /var/www/myblog/html/
sudo chown -R www-data:www-data /var/www/myblog/html
```

---

## 第七步：配置 Nginx

### 7.1 创建站点配置

```bash
sudo nano /etc/nginx/sites-available/myblog
```

```nginx
server {
    listen 80;
    server_name 你的域名.com;          # 替换为你的域名。若备案未完成，先用公网 IP（如 47.xx.xx.xx）

    # 前端静态文件根目录
    root /var/www/myblog/html;
    index index.html;

    # SPA 路由回退（Vue Router history 模式）
    location / {
        try_files $uri $uri/ /index.html;
    }

    # API 反向代理到 .NET 后端
    location /api/ {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection "keep-alive";
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
        proxy_cache_bypass $http_upgrade;
        proxy_read_timeout 60s;
    }

    # 静态资源长缓存
    location ~* \.(js|css|png|jpg|jpeg|gif|ico|svg|woff|woff2|ttf|eot)$ {
        expires 30d;
        add_header Cache-Control "public, immutable";
    }

    # 日志
    access_log /var/log/nginx/myblog-access.log;
    error_log /var/log/nginx/myblog-error.log;
}
```

### 7.2 启用站点

```bash
# 创建软链接
sudo ln -s /etc/nginx/sites-available/myblog /etc/nginx/sites-enabled/

# 删除默认站点（可选）
sudo rm -f /etc/nginx/sites-enabled/default

# 测试配置
sudo nginx -t

# 重载 nginx
sudo systemctl reload nginx
```

---

## 第八步：配置 HTTPS（Let's Encrypt）

> **前提**：
> - 域名已完成 DNS 解析，指向服务器公网 IP
> - **路线 A（国内云）**：ICP 备案必须已完成并通过，否则 Let's Encrypt 验证会失败
> - **路线 B（海外云）**：无限制，域名解析生效后即可操作
>
> 若备案尚未完成，可跳过此步，先通过 `http://公网IP` 访问测试。备案通过后再回来配置 HTTPS。

```bash
# 安装 certbot
sudo apt install -y certbot python3-certbot-nginx

# 自动申请证书并配置 HTTPS
sudo certbot --nginx -d 你的域名.com

# 测试自动续期
sudo certbot renew --dry-run
```

certbot 会自动修改 nginx 配置，将 HTTP 重定向到 HTTPS，无需手动调整。

---

## 第九步：防火墙配置

```bash
# 开放必要端口
sudo ufw allow 22/tcp    # SSH
sudo ufw allow 80/tcp    # HTTP
sudo ufw allow 443/tcp   # HTTPS

# 确保 MySQL 不对外暴露
sudo ufw deny 3306/tcp

# 启用防火墙
sudo ufw enable
sudo ufw status
```

---

## 第十步：验证部署

```bash
# 1. 后端直接访问
curl http://localhost:5000/api/status

# 2. 前端通过 nginx
curl http://localhost/

# 3. API 通过 nginx 代理
curl http://localhost/api/status

# 4. 检查服务状态
sudo systemctl status myblog-api
sudo systemctl status nginx
sudo systemctl status mysql
```

### 浏览器验证

| 场景 | 访问地址 |
|---|---|
| 域名 + HTTPS 已配置 | `https://你的域名.com` |
| 域名已解析，HTTPS 未配置 | `http://你的域名.com` |
| 备案等待中（国内路线） | `http://你的公网IP` |

### 备案等待期间的特殊处理（路线 A）

ICP 备案审核期间，域名无法通过 HTTP/HTTPS 正常访问。此时：

1. **用 IP 直接访问**：nginx 配置中 `server_name` 使用 IP 或添加 `_`（匹配所有请求）
2. **临时修改 nginx 配置**，将 `server_name` 改为 `_`：

   ```nginx
   server {
       listen 80;
       server_name _;         # 匹配任何请求（IP 和域名均可）
       ...
   ```

3. 备案通过后，将 `server_name` 改回你的域名，然后执行 `sudo certbot --nginx` 配置 HTTPS



---

## 日常更新流程

代码更新后执行以下步骤：

```bash
# === 后端更新 ===
cd /var/www/myblog
git pull

# 如果有数据库变更
cd blog_server
dotnet ef database update --project blog_server/blog_server.csproj
cd ..

# 重新发布
dotnet publish blog_server/blog_server/blog_server.csproj \
    -c Release -r linux-x64 --sc false \
    -o /var/www/myblog/publish

# 重启服务
sudo systemctl restart myblog-api

# === 前端更新 ===
cd /var/www/myblog/web_page
npm install --production
npm run build
sudo rm -rf /var/www/myblog/html/*
sudo cp -r dist/* /var/www/myblog/html/
sudo systemctl reload nginx
```

---

## 故障排查

### 502 Bad Gateway

后端未运行或端口不对：

```bash
sudo systemctl status myblog-api
curl http://localhost:5000/api/status
sudo journalctl -u myblog-api -n 30 --no-pager
```

### 数据库连接失败

```bash
# 测试数据库连接
mysql -u blog_user -p -h localhost blog -e "SHOW TABLES;"

# 检查 appsettings.Production.json 中的连接字符串
cat /var/www/myblog/blog_server/blog_server/appsettings.Production.json
```

### 前端页面空白 / 刷新 404

检查 nginx SPA 路由配置是否包含 `try_files $uri $uri/ /index.html;`。

### 修改代码后不生效

```bash
# 确认构建成功
npm run build
ls -la /var/www/myblog/html/

# 强制刷新浏览器缓存：Ctrl + Shift + R
```

---

## 安全建议

1. **Admin Token**：务必修改 `appsettings.Production.json` 中的默认 `Admin:Token`，生产环境不要使用 `dev-admin-token-change-me`
2. **Admin Password**：同样不要使用默认的 `blog-admin`
3. **数据库密码**：使用强密码，限制本地访问（`blog_user@'localhost'`）
4. **SSH**：建议使用密钥登录，禁用密码登录
5. **防火墙**：仅开放 22、80、443 端口，MySQL 3306 端口禁止公网访问
6. **日志**：定期检查 `/var/log/nginx/` 和 `journalctl` 中的异常请求
7. **HTTPS**：确保 Let's Encrypt 自动续期正常工作（`sudo certbot renew --dry-run`）
8. **备案信息**（路线 A）：ICP 备案通过后，在网站底部添加备案号，格式为 `© 2026 MyBlog | ICP备xxxxxx号`
9. **server_name**：备案通过后，及时将 nginx 中临时的 `server_name _` 改回你的域名

---

## 附录：一键更新脚本

将以下内容保存为 `/var/www/myblog/update.sh`，赋予执行权限后使用：

```bash
#!/bin/bash
set -e

echo ">>> 拉取最新代码..."
cd /var/www/myblog
git pull

echo ">>> 更新后端..."
cd blog_server
dotnet ef database update --project blog_server/blog_server.csproj
cd ..
dotnet publish blog_server/blog_server/blog_server.csproj \
    -c Release -r linux-x64 --sc false \
    -o /var/www/myblog/publish
sudo systemctl restart myblog-api
echo ">>> 后端更新完成"

echo ">>> 更新前端..."
cd /var/www/myblog/web_page
npm install --production
npm run build
sudo rm -rf /var/www/myblog/html/*
sudo cp -r dist/* /var/www/myblog/html/
sudo systemctl reload nginx
echo ">>> 前端更新完成"

echo ">>> 全部更新完成！"
```

```bash
chmod +x /var/www/myblog/update.sh
./update.sh
```
