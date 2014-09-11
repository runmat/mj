
cd inetpub/wwwroot/Portal
tf get $/Migration/CKAG/portal /recursive /force

cd Applications
tf get $/Migration/CKAG/Applications /recursive /force

cd ..
cd Components
tf get $/Migration/CKAG/Components /recursive /force
