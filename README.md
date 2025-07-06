# SetListr

SetListr is a tiny, cross-platform app for managing musical setlists. Built with .NET 9, MAUI, and Blazor, it runs on desktop, web, and mobile, providing a modern and flexible experience for musicians and organizers.

## Features

- Create, edit, and manage setlists
- Organize songs, albums, and artists
- Cross-platform: runs on Windows, macOS, Linux, Android, iOS, and WebAssembly
- Modern UI using Fluent UI and Blazor
- Local database storage with SQLite (on supported platforms)
- Open source and extensible

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- (Optional) Visual Studio 2022 or later with MAUI and Blazor workloads

### Running the App

#### MAUI (Desktop/Mobile)

1. Clone the repository:```sh
git clone https://github.com/ryzngard/SetListr.git
cd SetListr
```2. Open the solution in Visual Studio.
3. Set `SetListr.MauiBlazor` as the startup project.
4. Run the app on your desired platform (Windows, macOS, Android, iOS).

#### Blazor WebAssembly

1. Navigate to the `src/SetListr.BlazorWasm` directory.
2. Run:```sh
dotnet run
```3. Open your browser to the provided local address.

## Project Structure

- `src/SetListr.MauiBlazor` - .NET MAUI Blazor app (desktop/mobile)
- `src/SetListr.BlazorWasm` - Blazor WebAssembly app (web)
- `src/SetListr.Razor` - Shared Blazor components and pages
- `src/SetListr.Data` - Data models and database context

## Contributing

Contributions are welcome! Please open an [issue](https://github.com/ryzngard/SetListr/issues) or submit a pull request.

## License

This project is licensed under the MIT License.

## Links

- [Source Code](https://github.com/ryzngard/SetListr)
- [File an Issue](https://github.com/ryzngard/SetListr/issues)
