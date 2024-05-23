using Olimpo.Domain;
using Olimpo.Sensors;
using Spectre.Console;

namespace Olimpo;

public class ConsoleExhibitor
{
    public static void Show(List<Stack> stacks)
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

            
        
            

            foreach (var service in stack.services)
            {
                

                // Create sendor grid 
                var grid = new Grid();
                grid.AddColumn().Width(450);
                grid.AddColumn().Width(50);
                grid.AddColumn().Width(50);
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
                    new Text("Status", new Style(Color.Blue, Color.Black))
                });
                
                foreach (var sensor in service.sensors)
                {
                    if(sensor.channels == null || sensor.channels.Count() == 0){
                        continue;
                    }
                    foreach (var channel in sensor.channels)
                    {
                        Metric metric = channel.metric;
                        
                        if(metric == null){
                            metric = new Metric(){
                                datetime = DateTime.Now
                            };
                        }

                        Style color = null;
                        //color = (metric.value == -1) ? new Style(Color.Red) : metric.value > 100 ? new Style(Color.Red) : metric.value >= 50 ? new Style(Color.Yellow) : new Style(Color.Green);

                        if(channel.alerts != null){
                            switch (channel.alerts.type)
                            {
                                case Alert.Type.exact: color = (metric.value == channel.alerts.critical) ? new Style(Color.Red) : metric.value == channel.alerts.warning ? new Style(Color.Yellow) : new Style(Color.Green);
                                break;

                                //TODO: implementar lower_better
                                case Alert.Type.lower_better: color = (metric.value == -1) ? new Style(Color.Red) : metric.value <= channel.alerts.success ? new Style(Color.Green) : metric.value <= channel.alerts.warning ? new Style(Color.Yellow) : new Style(Color.Red);
                                break;

                                //TODO: implementar upper_better
                                case Alert.Type.upper_better: color = (metric.value == -1) ? new Style(Color.Red) : metric.value < 50 ? new Style(Color.Red) : metric.value < 100 ? new Style(Color.Yellow) : new Style(Color.Red);
                                break;

                                default: break;
                            }
                        }
                        
                        var value = (metric.value == -1) ? "-" : Sensor.NumberToCorrectUnit(metric.value, sensor.metric_unit).ToString("0.##");
                        string unit = (sensor.metric_unit == Sensor.MetricUnit.None) ? "" : sensor.metric_unit.ToString();
                    
                        // Add content row 
                        grid.AddRow(new Text[]{
                            new Text(channel.name).LeftJustified(),
                            new Text(sensor.type.ToString()),
                            new Text(sensor.port.ToString()),
                            new Text($"{metric.latency.ToString()} ms"),
                            new Text($"{value} {unit}", color),
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
