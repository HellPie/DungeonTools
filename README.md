# DungeonTools

Libraries and tools to interact with [Minecraft: Dungeons](https://www.minecraft.net/en-us/about-dungeons/) save files.

## How to use it

> **WARNING**: The tool overwrites existing files by default. Run it manually with `--overwrite=false` to change this behavior.

#### Automagic Mode™

This is the easy mode:
- Drag the files on top of the executable file and drop them, a blank window will open while the process runs.
- Once the window closes you will find the converted files in the same folder as the input files with their appropriate names.

#### Manual Mode

This mode is for users who are not afraid of "ugly", black and white text-only interfaces.
- Open a "Command Prompt" or "PowerShell" window (from now own referred as "CLI") and move to the folder where the tool has been downloaded.
- Execute the tool manually calling `.\dtool.exe <input>` replacing `<input>` with the paths to the desired files, separated by spaces.
    - Execute the tool as `.\dtool.exe <input> --overwrite=false` to instruct the tools to not overwrite existing files.
    - You can also drag and drop the files into the CLI to let Windows provide the path to the file on its own, make sures files are still separated by a single space.
- Press enter to execute the program and wait until execution is completed an a new terminal line shows awaiting for input.
 
## Downloads

The latest release is currently available through [GitHub Releases](https://github.com/HellPie/DungeonTools/releases) for this repository.

## Build it Yourself

You can build this repository by cloning it and running `dotnet restore`, `dotnet build` and `dotnet publish`.

Refer to the [official `dotnet` documentation](https://docs.microsoft.com/en-us/dotnet/core/tools/) to learn how to use the `dotnet` tooling. This repository will not provide further instructions.

#### The secret sauce:
<details>
<summary>The commands I use and you may want to copy and paste if you know what you're doing</summary>
---

To compile the CLI tool I simply run:
> `dotnet publish .\src\DungeonTools.Cli\DungeonTools.Cli.csproj --configuration Release --output .\out\`

To compile the server the process relies on the provided [`Dockerfile`](./Dockerfile) and some `docker-compose` configuration to put it behind a reverse proxy with HTTPS support.

To have a working server you'll need the keys. The server currently serves the encryption keys through the [`api/encryption/keys` endpoint](https://dungeons.tools/api/encryption/keys).
You will notice it always answers with a `HTTP Status Code 451: Unavailable for Legal Reasons`. That happens because the AES Key and IV are Mojang's private property and nobody is legally allowed to share them.
</details>

## Legal Disclaimer

This project is not affiliated with Mojang Studios, XBox Game Studios, Double 11 or the Minecraft brand.

"Minecraft" is a trademark of Mojang Synergies AB.

Other trademarks referenced herein are property of their respective owners.

## License

This project is licensed under the [AGPL 3.0](LICENSE):

```
Copyright (C) 2020 Diego Rossi (https://github.com/HellPie)

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License as
published by the Free Software Foundation, either version 3 of the
License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with this program. If not, see <https://www.gnu.org/licenses/>.
```

## Third party libraries:

This project relies on the following third party libraries:

#### [CommandLine](https://github.com/commandlineparser/commandline)
Command Line parser library for CLR and NetStandard

Released under the [MIT License](https://github.com/commandlineparser/commandline/blob/master/License.md):
```
The MIT License (MIT)

Copyright (c) 2005 - 2015 Giacomo Stelluti Scala & Contributors

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
of the Software, and to permit persons to whom the Software is furnished to do
so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
```
