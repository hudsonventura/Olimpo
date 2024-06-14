# Olimpo - Network Monitoring
At the top, you see everything <small>(Everything means hardware and software only)</small>
![alt text](image.png)  
<br>





# Instalation
Just `docker compose up`, after see `http://localhost:3000`. Easy peasy lemon squeezy =)

<br>





# TODO
 - Implement sensor compatible with PRTG Advanced Script;
 - Implement edit sensor;
 - Implement edit channel;
 - Implement docker-compose.yml;
 - Implement a screen to edit the name of the channel;
 - Implement a screen to edit the limits of the channel;
 - Implement a way to read the limits and trigger a alarm;
 - Implement a way to copy the Advanced Script to the server target;
 - Implement a way to monitor the Windows (© Microsoft), CPU, memory, list of apps, disk, temps, etc;
 - Reactivate the list error screen;
 - At the class SensorsChecker and function LoopCheck, when fullClassName == null, save to the db the info that the sensor was not implemented;
 - Correct the case of all sensors class name;
 - Implement sensor CPU;
 - Unhardcode the container name from sensor SSH_DOCKER_CONTAINER;
 - Unhardcode the script from sensor SSHAdvancedScript;
 - Unhardcode the script from sensor Memory;

# Done