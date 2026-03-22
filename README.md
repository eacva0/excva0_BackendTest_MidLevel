# excva0_BackendTest_MidLevel

## 專案架構 (Project Architecture)

本專案使用 **.NET 8** 建立，採用 **ASP.NET Core Web API (Minimal APIs)** 架構進行開發，並透過 **Entity Framework Core** 進行資料存取。

### 目錄結構

- `ACPD/`：應用程式主要原始碼目錄
  - `Endpoints/`：包含 API 路由定義與商業邏輯處理 (如 `MyOfficeAcpdEndpoints.cs`)。
  - `Models/`：資料表實體層 (Entities) 以及 DTO/Request 物件 (如 `MyOffice_ACPD`, `MyOfficeAcpdUpsertRequest`)。
  - `Data/`：資料庫內容配置與設定 (包含 `MercuryTestDbContext`)。
  - `ACPD.http`：提供以文字方式進行 API 快速測試的檔案。
  - `appsettings.json`：包含應用程式的參數設定，例如資料庫連線字串 (`ConnectionStrings:MercuryTestDb`)。
- `MercurTest.bak`：SQL Server 資料庫備份檔，包含運行本測試所需的資料表與範例資料。

## 執行步驟 (Execution Steps)

### 1. 資料庫建置 (Database Setup)
1. 開啟 SQL Server Management Studio (SSMS) 或 Azure Data Studio。
2. 將專案根目錄中的 `MercurTest.bak` 備份檔進行**資料庫還原 (Restore Database)**。
3. 預設的資料庫名稱需設定為 `MercuryTestDb`。
4. (選用) 預設配置的連線字串為 `Server=(localdb)\MSSQLLocalDB`，如果您使用的是不同的 SQL Server 執行個體，請至 `ACPD/appsettings.json` 修改 `ConnectionStrings:MercuryTestDb` 對應您的資料庫連線設定。

### 2. 回復相依套件與建置 (Restore & Build)
可使用 Visual Studio (2022+) 開啟根目錄的 `ACPD.slnx` 方案檔，或透過命令列介面：
```bash
# 切換至 API 專案目錄
cd ACPD

# 還原 NuGet 套件
dotnet restore

# 建置專案
dotnet build
```

### 3. 執行應用程式 (Run)
```bash
# 執行專案
dotnet run
```
應用程式啟動後，將會顯示監聽的 `http://localhost:7136` 與 `https://localhost:5038` 網址。

### 4. 進行測試 (Testing)
- **Swagger UI**：使用瀏覽器開啟應用程式網址，並切換至 `/swagger` (例如 `https://localhost:xxxx/swagger`) 進行視覺化 API 檢視與測試。