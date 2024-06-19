using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace agent.Controllers;

[ApiController]
[Route("[controller]")]
public class CPUController : ControllerBase
{
     
    [HttpGet("/teste")]
    public string Get(){


        
        return "ok";
    }

    [HttpGet("/teste2")]
    public async Task<IActionResult> GetCpuStats()
    {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return await GetLinuxCpuStats();
            }
            else
            {
                return StatusCode(501, "Unsupported platform");
            }
        }

        private async Task<IActionResult> GetLinuxCpuStats()
        {
            var cpuUsages = new List<float>();
            var initialReadings = ReadCpuStats();
            await Task.Delay(1000); // Espera 1 segundo entre as leituras
            var finalReadings = ReadCpuStats();

            if (initialReadings.Count == finalReadings.Count)
            {
                for (int i = 0; i < initialReadings.Count; i++)
                {
                    var initial = initialReadings[i];
                    var final = finalReadings[i];

                    float idleDiff = final.Idle - initial.Idle;
                    float totalDiff = (final.User + final.Nice + final.System + final.Idle + final.Iowait + final.Irq + final.Softirq)
                                    - (initial.User + initial.Nice + initial.System + initial.Idle + initial.Iowait + initial.Irq + initial.Softirq);

                    float usage = (totalDiff - idleDiff) / totalDiff * 100;
                    cpuUsages.Add(usage);
                }
            }

            return Ok(cpuUsages);
        }

        private List<CpuStat> ReadCpuStats()
        {
            var lines = System.IO.File.ReadAllLines("/proc/stat");
            var stats = new List<CpuStat>();

            foreach (var line in lines)
            {
                if (line.StartsWith("cpu") && !line.StartsWith("cpu "))
                {
                    var columns = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (columns.Length >= 8)
                    {
                        stats.Add(new CpuStat
                        {
                            User = float.Parse(columns[1]),
                            Nice = float.Parse(columns[2]),
                            System = float.Parse(columns[3]),
                            Idle = float.Parse(columns[4]),
                            Iowait = float.Parse(columns[5]),
                            Irq = float.Parse(columns[6]),
                            Softirq = float.Parse(columns[7])
                        });
                    }
                }
            }

            return stats;
        }
    }
public class CpuStat
{
    public float User { get; set; }
    public float Nice { get; set; }
    public float System { get; set; }
    public float Idle { get; set; }
    public float Iowait { get; set; }
    public float Irq { get; set; }
    public float Softirq { get; set; }
}
    

