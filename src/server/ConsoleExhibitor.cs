using Microsoft.EntityFrameworkCore;
using Olimpo.Domain;
using Olimpo.Sensors;
using Spectre.Console;

namespace Olimpo;

public class ConsoleExhibitor
{
    public static void StartLoopShow(IConfiguration appsettings){
        using (var db = new Context(appsettings)){
            while(true){
                List<Stack> stacks = db.stacks
                    .AsNoTracking()
                    .Include(x => x.devices)
                    .ThenInclude(x => x.sensors)
                    .ThenInclude(x => x.channels)
                    .ThenInclude(x => x.current_metric)
                    .ToList(); 

                ConsoleExhibitor.Show(stacks);
                Thread.Sleep(3000);
            }
        }
    }
    private static void Show(List<Stack> stacks)
    {
        // Create the layout
        Layout layout = new Layout("Root")
            .SplitColumns(
                new Layout("Stacks").Ratio(9).SplitRows(
                        new Layout("Top")
                        //, new Layout("Bottom")
                )
            );
                    
        /* layout["Bottom"].Size(3);
        layout["Bottom"].Update(
            new Panel(
                Align.Center(
                    new Markup("Hello [blue]World![/]")
                )
            )
        ); */

        Tree root = new Tree("Stacks");
        

        foreach (var stack in stacks)
        {
            var stackNode = root.AddNode($"[bold][blue][[{stack.name}]][/][/]");

            
        
            

            foreach (var service in stack.devices)
            {
                

                // Create sendor grid 
                var grid = new Grid();
                grid.AddColumn().Width(450);
                grid.AddColumn().Width(50);
                grid.AddColumn().Width(50);
                grid.AddColumn().Width(70);
                grid.AddColumn().Width(70);
                grid.AddColumn().Width(70);
                grid.AddColumn().Width(400);

                // Add header row 
                grid.AddRow(new Text[]{
                    new Text("Sensor", new Style(Color.Blue, Color.Black)),
                    new Text("Type", new Style(Color.Blue, Color.Black)),
                    new Text("Port", new Style(Color.Blue, Color.Black)),
                    new Text("Latency", new Style(Color.Blue, Color.Black)),
                    new Text("Value", new Style(Color.Blue, Color.Black)),
                    new Text("Last check", new Style(Color.Blue, Color.Black)),
                    new Text("Status", new Style(Color.Blue, Color.Black))
                });
                
                foreach (var sensor in service.sensors)
                {
                    if(sensor.channels == null || sensor.channels.Count() == 0){
                        continue;
                    }
                    foreach (var channel in sensor.channels)
                    {
                        Metric metric = channel.current_metric;
                        
                        if(metric == null){
                            metric = new Metric(){
                                datetime = DateTime.UtcNow
                            };
                        }

                        Style color = null;
                        //color = (metric.value == -1) ? new Style(Color.Red) : metric.value > 100 ? new Style(Color.Red) : metric.value >= 50 ? new Style(Color.Yellow) : new Style(Color.Green);

                        switch (channel.current_metric.status)
                        {
                            case Metric.Status.NotChecked: color = new Style(Color.DarkRed);
                            break;

                            case Metric.Status.Success: color = new Style(Color.Green);
                            break;

                            case Metric.Status.Paused: color = new Style(Color.Blue);
                            break;

                            case Metric.Status.Warning: color = new Style(Color.Yellow);
                            break;

                            case Metric.Status.Error: color = new Style(Color.Red);
                            break;

                            default: break;
                        }
                        
                        var valuestring = metric.value?.ToString("0.##") ?? string.Empty;
                        var value = (metric.value == -1) ? "-" : valuestring;


                    
                        // Add content row 
                        grid.AddRow(new Text[]{
                            new Text(channel.name).LeftJustified(),
                            new Text(sensor.type.ToString()),
                            new Text(sensor.port.ToString()),
                            new Text($"{metric.latency.ToString()} ms"),
                            new Text($"{value} {channel.unit}", color),
                            new Text(metric.datetime.ToString("yyyy/MM/dd HH:mm:ss"), color),
                            new Text(metric.message, color)
                        });
                    }
                }
                var serviceNode = stackNode.AddNode(new Text($"{service.name} - {service.host}", new Style(Color.Blue, Color.Black)));
                serviceNode.AddNode(grid);
            }
            ;
            // Update the Stacks column
            layout["Top"].Update(
                 new Panel(root)
                     .Expand());
            AnsiConsole.Write(layout);
        }

        

        


 

        
    }
}
