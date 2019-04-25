::set STEAMDIR="C:\\Program Files (x86)\\Steam\\steamapps\\common\\The Scroll Of Taiwu\\"
::set STEAMDIR="E:\\SteamLibrarys\\steamapps\\common\\The Scroll Of Taiwu\\"
set STEAMDIR="F:\\steam\\steamapps\\common\\The Scroll Of Taiwu\\The Scroll Of Taiwu Alpha V1.0_Data"
mkdir build
cd build

cmake -G "Visual Studio 15 2017" -D STEAMDIR=%STEAMDIR%  ..

cd ..
