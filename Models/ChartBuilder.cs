using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.UI.DataVisualization.Charting;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace SMoDABot.Models
{
    public static class ChartBuilder
    {
        public static void BuildChartRelFreqPoligonFreq(double[] Elements, Chart chart)
        {
            chart.Series.Clear();
            chart.ChartAreas.Clear();

            chart.ChartAreas.Add("ChartArea1");

            chart.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            chart.ChartAreas["ChartArea1"].AxisY.Interval = 1;
            chart.ChartAreas["ChartArea1"].AxisX.ArrowStyle = AxisArrowStyle.Triangle;
            chart.ChartAreas["ChartArea1"].AxisY.ArrowStyle = AxisArrowStyle.Triangle;

            chart.Series.Add("Poligon");
            chart.Series["Poligon"].IsXValueIndexed = true;
            chart.Series["Poligon"].BorderWidth = 3;
            chart.Series["Poligon"].ChartType = SeriesChartType.Line;

            SMoDALib.CRow Row = new SMoDALib.CRow(new List<double>(Elements));
            int length = Row.UniqLength();
            for (int i = 0; i < length; ++i)
                chart.Series["Poligon"].Points.AddXY(Row.Elements[i].numb, Row.Elements[i].rel_frequency);
        }

        public static System.IO.Stream GetFreqPoligonFreqChartStream(double[] Elements)
        {
            Chart chart = new Chart();
            ChartBuilder.BuildChartRelFreqPoligonFreq(Elements, chart);

            System.IO.Stream stream = new System.IO.MemoryStream();
            chart.SaveImage(stream, ChartImageFormat.Png);
            chart.SaveImage("chart", ChartImageFormat.Png);
            return stream;
        }
        public async static Task SendChart(CallbackQuery callback, TelegramBotClient client)
        {
            try
            {
                double[] Elements = { 1, 2, 3,3,4,5,6,8,4,7,8,9,6,3,3,1,2,5,4 };
                Chart chart = new Chart();
                SMoDALib.StatisticCharts.BuildChartRelFreqPoligonFreq(Elements, chart);
                string path = HostingEnvironment.ApplicationPhysicalPath + String.Format("chartimagewg{0}.png",callback.Message.Chat.Id);
                
                var s = new System.IO.FileStream(path, System.IO.FileMode.OpenOrCreate);
                chart.SaveImage(s, ChartImageFormat.Png);
                s.Close();
                using (var stream = System.IO.File.Open(path, System.IO.FileMode.Open))
                {
                    InputOnlineFile iof = new InputOnlineFile(stream);
                    iof.FileName = "Chart.png";
                    var send = await client.SendDocumentAsync(callback.Message.Chat.Id, iof, "Chart");
                    stream.Close();
                    System.IO.File.Delete(path);
                }
            }
            catch (Exception ex)
            {
                Exception exc = ex;
                string s = ex.Message;
                await client.SendTextMessageAsync(chatId: callback.Message.Chat.Id, text: s);
            }
        }
        
    }
}