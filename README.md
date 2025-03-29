# CLI Caching Proxy with ASP.NET

## Overview

This project is a CLI-based caching proxy built with ASP.NET. It forwards requests to an actual server and caches the responses. If the same request is made again within a short period, the proxy returns the cached response instead of forwarding the request to the server, improving performance and reducing redundant server load.

## Features

- Request Forwarding: Sends incoming requests to the target server and retrieves responses.

- Response Caching: Stores responses in Redis to serve repeated requests efficiently.

- Optimized Performance: Reduces redundant requests and improves response times.

-  Easy to set up and use via CLI.

---

## 📥 **Installation**

### **1️⃣ Clone the repository**
```sh
git clone https://github.com/YOUR_USERNAME/YOUR_REPO.git
cd YOUR_REPO
```

### **2️⃣ Install dependencies**
Make sure you have **.NET 7+** and **Redis** installed.

#### **Install Redis (if not already installed)**
- **Windows (via Docker)**
  ```sh
  docker run --name redis -p 6379:6379 -d redis
  ```
- **Mac/Linux (via Homebrew)**
  ```sh
  brew install redis
  redis-server
  ```

#### **Install .NET dependencies**
```sh
dotnet restore
```

---

## 🚀 **Usage**

### **Start the Proxy**
```sh
dotnet run --port 3000 --target "https://jsonplaceholder.typicode.com or your own API"
```
- `--port 3000` → Proxy listens on **http://localhost:3000**  
- `--target` → The server it will forward requests to  

### **Example Request**
Once running, send a request via **curl** or Postman:
```sh
curl http://localhost:3000/posts/1
```
- The first request **fetches data from the real server**.  
- The second request **returns the cached response** (faster).  

### **Customizing Cache Expiry**
Modify `CacheService.cs` to set a custom expiration:
```csharp
await _cacheService.SetAsync(cacheKey, responseBody, expiry: new System.TimeSpan(0, 0, ttl));
```
- Change `System.TimeSpan(hours, minutes, seconds)` to any value to adjust caching time.

---

## 🛠️ **Building and Packaging**

To create a **self-contained executable**:
```sh
dotnet publish -c Release -r win-x64 --self-contained true -o ./build
```
- For Windows: `-r win-x64`
- For Linux: `-r linux-x64`
- For Mac: `-r osx-x64`

Then run the compiled executable:
```sh
./build/YourProxyCLI --port 3000 --target "https://jsonplaceholder.typicode.com"
```
