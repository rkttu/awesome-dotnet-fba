# Awesome .NET File-based Apps 🚀

A curated collection of awesome code samples for .NET 10's File-based Apps feature.

[![.NET](https://img.shields.io/badge/.NET-10.0-purple.svg)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![GitHub Sponsors](https://img.shields.io/github/sponsors/rkttu?style=flat&logo=github&logoColor=white&labelColor=gray&color=pink)](https://github.com/sponsors/rkttu)

## 📋 Sample Overview

| Directory | Sample Files | Description | Key Technologies |
|-----------|--------------|-------------|------------------|
| **001-hello-world** | [HelloWorld.cs](001-hello-world/HelloWorld.cs), [HelloWorldShebang.cs](001-hello-world/HelloWorldShebang.cs), [HelloWorldShebangAlt.cs](001-hello-world/HelloWorldShebangAlt.cs), [AspxSample.cs](001-hello-world/AspxSample.cs) | Basic Hello World examples and ASPX-style file-based web applications | .NET 10, Top-level programs, Shebang scripts |
| **002-win32-app-example** | [nightcurtain.cs](002-win32-app-example/nightcurtain.cs), [nativedll.cs](002-win32-app-example/nativedll.cs), [rundllsample.cs](002-win32-app-example/rundllsample.cs) | Windows API integration and native DLL interop examples | Win32 API, P/Invoke, Windows Registry |
| **003-gen-ai-example** | [ollama-kanana.cs](003-gen-ai-example/ollama-kanana.cs), [mcp-server.cs](003-gen-ai-example/mcp-server.cs) | AI chatbot and Model Context Protocol server implementations | Semantic Kernel, Ollama, MCP, AI Integration |
| **004-windows-forms** | [windowsforms.cs](004-windows-forms/windowsforms.cs) | Basic Windows Forms GUI application | Windows Forms, STA Threading |
| **005-wpf** | [wpf.cs](005-wpf/wpf.cs) | WPF application with MVVM pattern | WPF, MVVM, CommunityToolkit.Mvvm |
| **006-wasm-without-blazor** | [wasmwithoutblazor.cs](006-wasm-without-blazor/wasmwithoutblazor.cs) | Pure WebAssembly without Blazor framework | WebAssembly, JavaScript Interop |
| **007-playwright** | [linkcrawler.cs](007-playwright/linkcrawler.cs) | Web automation and link crawling | Playwright, Web Scraping |
| **008-aspire** | [wordpress.cs](008-aspire/wordpress.cs) | Cloud-native application orchestration | .NET Aspire, Docker, WordPress, MySQL |
| **009-aspnet-mvc-razor-pages** | [MvcTest.cs](009-aspnet-mvc-razor-pages/MvcTest.cs) | ASP.NET Core MVC with Razor Pages | ASP.NET Core, MVC, Razor Pages |
| **010-dotnet-tool** | [sampletool.cs](010-dotnet-tool/sampletool.cs) | .NET global tool creation | .NET Tools, NuGet Packaging |
| **011-aws-cdk** | [cdksample.cs](011-aws-cdk/cdksample.cs) | AWS infrastructure as code | AWS CDK, Cloud Infrastructure |
| **012-pythonnet** | [pythonnet.cs](012-pythonnet/pythonnet.cs) | Python.NET integration | Python.NET, Language Interop |
| **013-embeddinggemma-sqlite-vec** | [embedding.cs](013-embeddinggemma-sqlite-vec/embedding.cs) | Embedding and vector database operations | SQLite, Vector Search, Embeddings |
| **014-netfx-with-log4net** | [Program.cs](014-netfx-with-log4net/Program.cs) | .NET Framework application with logging | .NET Framework, log4net |
| **015-ikvm-mvn-korean-nlp** | [Program.cs](015-ikvm-mvn-korean-nlp/Program.cs) | Korean NLP using Java libraries via IKVM.NET | IKVM.NET, Korean NLP, Java Interop |

## 📖 What are File-based Apps

File-based Apps is an innovative feature introduced in .NET 10 that allows you to run applications with a single `.cs` file without the need for traditional project files (.csproj).

### 🎯 Key Features

- **Zero Configuration**: Run instantly without project files
- **Simplified Deployment**: Easy deployment with single files
- **Enhanced Performance**: Optimized execution speed
- **Top-level Programs**: Write more concise code
- **Automatic Package Resolution**: Smart package management

## 🛠️ Development Environment Setup

### Prerequisites

- .NET 10.0 SDK or later
- Your preferred text editor or IDE
  - **Visual Studio Code**: Lightweight development environment
  - **Any VSCode OSS-based Editors**: Cursor, Amazon Kiro, Windsurf, Eclipse Theia, etc.
- C# Extension for VS Code (Roslyn/OmniSharp LSP included)
  - [For Visual Studio Code](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp)
  - [For Any VSCode OSS-based forks (Including Cursor, Amazon Kiro, Windsurf, Eclipse Theia, etc.)](https://open-vsx.org/extension/dotnetdev-kr-custom/csharp)

## 🚀 Quick Start

To run any sample, navigate to the directory and execute one of the following commands:

```bash
# Basic execution method
dotnet run filename.cs

# For files with shebang (Unix/Linux/macOS)
chmod +x filename.cs
./filename.cs

# Windows PowerShell
Get-Content -Path '.\filename.cs' | dotnet run -
```

## 📂 Detailed Sample Descriptions

### 001-hello-world

Basic Hello World examples demonstrating different execution methods.

- **HelloWorld.cs** - The simplest Hello World program
- **HelloWorldShebang.cs** - Unix/Linux style shebang executable script
- **HelloWorldShebangAlt.cs** - Alternative shebang implementation
- **AspxSample.cs** - ASPX-style file-based web application

### 002-win32-app-example

Windows API and native DLL integration examples.

- **nightcurtain.cs** - Night mode utility controlling Windows color filters
- **nativedll.cs** - Native DLL calling example
- **rundllsample.cs** - Windows system function execution using rundll32

### 003-gen-ai-example

Generative AI and Model Context Protocol examples.

- **ollama-kanana.cs** - AI chatbot using Semantic Kernel and Ollama
- **mcp-server.cs** - Model Context Protocol (MCP) server implementation

### 004-windows-forms

Windows Forms application example.

- **windowsforms.cs** - Basic Windows Forms GUI application

### 005-wpf

WPF (Windows Presentation Foundation) application example.

- **wpf.cs** - Simple counter application using MVVM pattern

### 006-wasm-without-blazor

WebAssembly example without using Blazor.

- **wasmwithoutblazor.cs** - Stopwatch application with pure WebAssembly and JavaScript interaction

### 007-playwright

Web automation and crawling example.

- **linkcrawler.cs** - Web link crawler using Playwright

### 008-aspire

.NET Aspire cloud-native application example.

- **wordpress.cs** - WordPress, MySQL, phpMyAdmin orchestration using Docker containers

### 009-aspnet-mvc-razor-pages

ASP.NET Core MVC and Razor Pages example.

- **MvcTest.cs** - Web application combining MVC and Razor Pages

### 010-dotnet-tool

.NET global tool creation example.

- **sampletool.cs** - .NET tool that can be distributed as a NuGet package

### 011-aws-cdk

AWS CDK (Cloud Development Kit) example.

- **cdksample.cs** - Example of defining AWS infrastructure as code

### 012-pythonnet

Python.NET integration example.

- **pythonnet.cs** - Calling Python libraries from .NET

### 013-embeddinggemma-sqlite-vec

Embedding and vector database example.

- **embedding.cs** - Embedding processing using SQLite and vector search

### 014-netfx-with-log4net

.NET Framework application with logging example.

- **Program.cs** - Traditional .NET Framework application

### 015-ikvm-mvn-korean-nlp

IKVM.NET integration with Java-based Korean NLP libraries.

- **Program.cs** - Korean text processing using KKMA and Open Korean Text libraries through IKVM.NET

## 🤝 Contributing

We welcome new samples and improvements to this repository!

### Contribution Guidelines

1. Fork this repository
2. Create a new branch (`git checkout -b feature/amazing-sample`)
3. Commit your changes (`git commit -am 'Add amazing sample'`)
4. Push to the branch (`git push origin feature/amazing-sample`)
5. Create a Pull Request

### Sample Writing Rules

- Write each sample as a single `.cs` file
- Include clear comments and explanations
- Provide complete, runnable examples
- Add sample descriptions to README.md

## 📚 Additional Resources

- [.NET 10 Official Documentation](https://docs.microsoft.com/dotnet/)
- [File-based Apps Guide](https://docs.microsoft.com/dotnet/core/whats-new/dotnet-10)
- [.NET Community](https://dotnet.microsoft.com/community)

## 📄 License

This project is licensed under the [MIT License](LICENSE).

## 🙏 Acknowledgments

Thanks to all contributors in the .NET community and the Microsoft .NET team.

---

⭐ If you find this repository useful, please give it a star!

[![GitHub stars](https://img.shields.io/github/stars/rkttu/awesome-dotnet-fba.svg?style=social)](https://github.com/rkttu/awesome-dotnet-fba/stargazers)
