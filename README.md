[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]




<!-- PROJECT TITLE -->
<div align="center">
	<h1>VR Third Eye</h1>
	<p>
		<a href="https://www.nexusmods.com/derailvalley/mods/762">Download Mod</a>
		·
		<a href="https://github.com/derail-valley-modding/template-umm/issues">Report Bug</a>
		·
		<a href="https://github.com/derail-valley-modding/template-umm/issues">Request Feature</a>
	</p>
</div>




<!-- TABLE OF CONTENTS -->
<details>
	<summary>Table of Contents</summary>
	<ol>
		<li><a href="#about-the-project">About The Project</a></li>
		<li><a href="#building">Building</a></li>
		<li><a href="#packaging">Packaging</a></li>
		<li><a href="#license">License</a></li>
	</ol>
</details>




<!-- ABOUT THE PROJECT -->

## About The Project

A mod for [Derail Valley][derail-valley-url] that adds a central camera when in VR mode.




<!-- BUILDING -->

## Building

Building the project requires some initial setup, after which running `dotnet build` will do a Debug build or running `dotnet build -c Release` will do a Release build.

### References Setup

After cloning the repository, some setup is required in order to successfully build the mod DLLs. You will need to create a new [Directory.Build.targets][references-url] file to specify your local reference paths. This file will be located in the main directory, next to ThirdEye.sln.

Below is an example of the necessary structure. When creating your targets file, you will need to replace the reference paths with the corresponding folders on your system. Make sure to include semicolons **between** each of the paths and no semicolon after the last path. Also note that any shortcuts you might use in file explorer—such as %ProgramFiles%—won't be expanded in these paths. You have to use full, absolute paths.
```xml
<Project>
	<PropertyGroup>
		<ReferencePath>
			C:\Program Files (x86)\Steam\steamapps\common\Derail Valley\DerailValley_Data\Managed\;
			C:\Users\whoami\source\repos\dv-camera-manager\build\
		</ReferencePath>
		<AssemblySearchPaths>$(AssemblySearchPaths);$(ReferencePath);</AssemblySearchPaths>
	</PropertyGroup>
</Project>
```

### Line Endings Setup

It's recommended to use Git's [autocrlf mode][autocrlf-url] on Windows. Activate this by running `git config --global core.autocrlf true`.




<!-- PACKAGING -->

## Packaging

To package a build for distribution, you can run the `package.ps1` PowerShell script in the root of the project. If no parameters are supplied, it will create a .zip file ready for distribution in the dist directory. A post build event is configured to run this automatically after each successful Release build.

Linux: `pwsh ./package.ps1`
Windows: `powershell -executionpolicy bypass .\package.ps1`


### Parameters

Some parameters are available for the packaging script.

#### -NoArchive

Leave the package contents uncompressed in the output directory.

#### -OutputDirectory

Specify a different output directory.
For instance, this can be used in conjunction with `-NoArchive` to copy the mod files into your Derail Valley installation directory.




<!-- LICENSE -->

## License

Source code is distributed under the MIT license.
See [LICENSE][license-url] for more information.




<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->

[contributors-shield]: https://img.shields.io/github/contributors/fauxnik/dv-vr-third-eye.svg?style=for-the-badge
[contributors-url]: https://github.com/fauxnik/dv-vr-third-eye/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/fauxnik/dv-vr-third-eye.svg?style=for-the-badge
[forks-url]: https://github.com/fauxnik/dv-vr-third-eye/network/members
[stars-shield]: https://img.shields.io/github/stars/fauxnik/dv-vr-third-eye.svg?style=for-the-badge
[stars-url]: https://github.com/fauxnik/dv-vr-third-eye/stargazers
[issues-shield]: https://img.shields.io/github/issues/fauxnik/dv-vr-third-eye.svg?style=for-the-badge
[issues-url]: https://github.com/fauxnik/dv-vr-third-eye/issues
[license-shield]: https://img.shields.io/github/license/fauxnik/dv-vr-third-eye.svg?style=for-the-badge
[license-url]: https://github.com/fauxnik/dv-vr-third-eye/blob/master/LICENSE
[references-url]: https://learn.microsoft.com/en-us/visualstudio/msbuild/customize-your-build?view=vs-2022
[autocrlf-url]: https://www.git-scm.com/book/en/v2/Customizing-Git-Git-Configuration#_formatting_and_whitespace
[derail-valley-url]: http://www.derailvalley.com
