# Olimpo - Network Monitoring
At the top, you see everything <small>(Everything means hardware and software only)</small>
![alt text](image.png)  
<br>





# Instalation
Just `docker compose up`, after see `http://localhost:3000`. Easy peasy lemon squeezy =)

<br>





# TODO
 - [ ] Implement sensor compatible with PRTG Advanced Script;
 - [ ] Implement edit sensor;
 - [ ] Implement edit channel;
 - [ ] Implement docker-compose.yml;
 - [ ] Implement a screen to edit the name of the channel;
 - [ ] Implement a screen to edit the limits of the channel;
 - [ ] Implement a way to read the limits and trigger a alarm;
 - [ ] Implement a way to copy the Advanced Script to the server target;
 - [ ] Implement a way to monitor the Windows (© Microsoft), CPU, memory, list of apps, disk, temps, etc;
 - [ ] Reactivate the list error screen;
 - [ ] At the class SensorsChecker and function LoopCheck, when fullClassName == null, save to the db the info that the sensor was not implemented;
 - [x] <strike>Correct the case of all sensors class name;</strike>;
 - [ ] Implement sensor CPU;
 - [ ] Unhardcode the container name from sensor SSH_DOCKER_CONTAINER;
 - [ ] Unhardcode the script from sensor SSHAdvancedScript;
 - [ ] Unhardcode the script from sensor Memory;
 - [ ] Migrate SSH_Memory to top, because there are data of swap;
 - [ ] Build the Olimpo Agent:
    * Sensor CPU with % used, temp and fan speed;
    * Sensor HD with total, used and free space, temp and smart;
    * Sensor network with speed up and down;
    * Sensor system with motherboard temp;
    * Sensor GPU with % used, temp and fan speed;

