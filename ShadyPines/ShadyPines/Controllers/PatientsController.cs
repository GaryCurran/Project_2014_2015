using System;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using ShadyPines.Models;
using Point = DotNet.Highcharts.Options.Point;

namespace ShadyPines.Controllers
{
    public class PatientsController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        private static int _passedId;
        // GET: Patients
        [Authorize]
        public ActionResult Index(string sarchString)
        // public ActionResult Index()
        {
            var res = from r in _db.Patients select r;
            if (!string.IsNullOrEmpty(sarchString))
            {
                res = res.Where(s => s.Name.Contains(sarchString));
            }
            return View(res);
            // return View(db.Patients.ToList());
        }

        public ActionResult All()
        {
            // ReSharper disable once Mvc.ViewNotResolved
            return View(_db.Patients.ToList());
        }

 

        public ActionResult Chart(int? id)
        {

            //Mine

            // ReSharper disable once ObjectCreationAsStatement
            new MedicalQuestion();
            //mq = db.MedicalQuestions.Where(p => p.patientID == id).SingleOrDefault();
            if (id != null)
            {
                _passedId = (int)id;
            }

            // Patients Details
            var pt = _db.Patients.SingleOrDefault(p => p.PatientID == _passedId);
            if (pt == null) throw new ArgumentNullException("id");
            //pt = db.Patients.Where(p => p.PatientID == id).SingleOrDefault();
            ViewBag.name = pt.Name;
            ViewBag.a = pt.Gender;
            ViewBag.b = pt.MedicalCard;
            ViewBag.c = pt.DoctorName;

            // get all daily scores
            // from DB and transfer to array
            var tot = from e in _db.MedicalQuestions
                      where e.patientID == id
                      select e;


            // get total amount of objects to set size of object arrays
            var size = tot.Count();

            var t = new string[size];
            var pos = 0;
            foreach (var it in tot)
            {
                t[pos] = it.Date.Date.ToShortDateString();
                pos++;
            }
            //DateTime date1 = new DateTime(2008, 6, 1, 7, 47, 0);
            //DateTime dateOnly = date1.Date;
            var dy = new string[size];
            for (var j = 0; j < size; j++)
            {

                dy[j] = t[j];

            }
            // Use total amount of daily reports
            // to size array
            // pass to chart as an object
            var numbers = new object[size];
            var i = 0;

            // holding the total daily scores
            foreach (var item in tot)
            {
                numbers[i] = item.DailyTotal;
                i++;
            }

            // Sleep Paterns for period
            var quest1 = new object[size];
            var count = 0;
            foreach (var item in tot)
            {
                quest1[count] = ((int)item.Question1 + 1) * 2;
                count++;
            }

            var topSleep = 0;



            for (var a = 0; a < size; a++)
            {
                if ((int)quest1[a] > topSleep)
                {
                    topSleep = (int)quest1[a];
                }
            }
            // calculate fall period
            var fall = 0;
            var fell = 0;
            var noFall = 0;
            foreach (var item in tot)
            {
                if (item.HasFallen.Equals(true))
                {
                    fell++;
                    fall++;
                }
                else
                {
                    noFall++;
                    fall++;
                }
            }

            // Fall charts
            var fallChart = new object[size];
            var counter = 0;
            foreach (var item in tot)
            {
                if (item.HasFallen.Equals(true))
                {
                    fallChart[counter] = -1;
                    counter++;
                }
                else
                {
                    fallChart[counter] = 1;
                    counter++;
                }
            }

            var fallChartSeverity = new object[size];

            for (var f = 0; f < fallChart.Length; f++)
            {
                if (fallChart[f].Equals(-1) )
                {
                    fallChartSeverity[f] = ((int)fallChart[f]) + (-1);
                }
                    else
	            {
                    fallChartSeverity[f] = ((int)fallChart[f]) + (1);
	            }
            }
            var didFall = new object[size];
            var notFall = new object[size];

            for (var b = 0; b < size; b++)
            {
                if (fallChart[b].Equals(-1))
                {
                    didFall[b] = fallChart[b];
                }
                else
                {
                    notFall[b] = fallChart[b];
                }
            }

            // Appetite for period
            var quest2 = new object[size];
            var count2 = 0;
            foreach (var item in tot)
            {
                quest2[count2] = ((int)item.Question2 + 1) * 2;
                count2++;
            }

            // Appetite for period
            var quest3 = new object[size];
            var count3 = 0;
            foreach (var item in tot)
            {
                quest3[count3] = ((int)item.Question3 + 1) * 2;
                count3++;
            }

            // resting position
            var quest4 = new object[size];
            var count4 = 0;
            foreach (var item in tot)
            {
                quest4[count4] = ((int)item.Question4 + 1) * 2;
                count4++;
            }
            // length of stay determined by size of array
            var days = new string[size];

            // display No. of days on X axis
            for (var j = 0; j < days.Length; j++)
            {
                days[j] = "Day " + (j + 1);
            }

            // create another array to run comparissions
            var n = numbers;

            // sum total daily scores
            var s = 0;
            for (var k = 0; k < numbers.Length; k++)
            {
                s += (int)n[k];
            }

            // get avg daily score
            @ViewBag.avg = s / n.Length;

            // avg daily score to pass to graph
            var avgerageScore = new object[size];

            // populate avgerageScore []
            for (var g = 0; g < days.Length; g++)
            {
                avgerageScore[g] = @ViewBag.avg;
            }
            // This is a testing comment!!!!!!!!!!!
            // total number of daily observations
            ViewBag.total = numbers;

            // total number of days in hospital

            ////////////////////////////////////////////SUMMARY   CHART  ///////////////////////////////
            var chart = new Highcharts("chart")
             .InitChart(new Chart
             {
                 Type = ChartTypes.Line,
                 BorderColor = Color.AliceBlue,
                 AlignTicks = true
             })            
                .SetXAxis(new XAxis
                {
                    
                    Categories = dy,
                    Labels = new XAxisLabels
                   {
                       Rotation = -45,
                       Align = HorizontalAligns.Right,
                       Style = "fontSize: '13px',fontFamily: 'Verdana, sans-serif'"
                   }

                })//days })
                    .SetTitle(new Title { Text = "Health Record of " + pt.Name })
                    .SetCredits(new Credits { Enabled = false})
                     .SetSubtitle(new Subtitle { Text = "Observation period from: " + dy[0] + " to: " + dy[dy.Length - 1] })
                    .SetLegend(new Legend())
                        .SetLabels(new Labels())
                     .SetTooltip(new Tooltip
                     {
                         Shared = true,
                         Crosshairs = new Crosshairs(true,true)
                     })
                .SetPlotOptions(new PlotOptions
                {
                    Pie = new PlotOptionsPie
                    {
                        Center = new[] { new PercentageOrPixel(40), new PercentageOrPixel(20) },
                        Size = new PercentageOrPixel(80),
                        ShowInLegend = true,
                        DataLabels = new PlotOptionsPieDataLabels { Enabled = false }

                    }
                })
               
                .SetYAxis(new YAxis { Min = 0 })

                 .SetSeries(new[]
                            {
                                new Series {Name = "Daily Total", Color = ColorTranslator.FromHtml ("Blue"), Id= " Hello" ,Data = new Data (numbers)},

                                 new Series {Name = "Sleep Pattterns",Color = ColorTranslator.FromHtml ("Purple"), Data = new Data (quest1)},

                                 new Series {Name = "Appetite",Color = ColorTranslator.FromHtml ("Orange"), Data = new Data (quest2)},

                                 new Series {Name = "Resident Awareness",Color = ColorTranslator.FromHtml ("Brown"), Data = new Data (quest3)},

                                new Series {Name = "Daily Average " , Color = ColorTranslator.FromHtml("Green"), Data = new Data (avgerageScore)},

                                new Series
                    {
                        Type = ChartTypes.Pie,
                        Name = "Falls Observation",
                        Data = new Data(new[]
                        {
                            new Point
                            {
                                Name = "Days falls occured",
                                Y = fell,
                                Color = Color.FromName("Red")
                            },
                            new Point
                            {
                                Name = "Days no falls occured" ,
                                Y = noFall ,
                                Color = Color.FromName("Green")
                            },
                          }
                      )
                    }
           });

            ///////////////////////////////////////////////FALLS CHART///////////////////////////////////////////////
            var chart5 = new Highcharts("chart5")
               .InitChart(new Chart { Type = ChartTypes.Column, 
                   BorderColor = Color.AliceBlue })
               .SetTitle(new Title { Text = "Days when fall occured" })
                .SetCredits(new Credits { Enabled = false })
                 .SetTooltip(new Tooltip
                 {
                     Enabled = false                     
                 })
                 .SetSubtitle(new Subtitle { Text = "Observation period from: " + dy[0] + " to: " + dy[dy.Length - 1] })
             .SetXAxis(new XAxis
             {
                 Categories = dy,
                 Labels = new XAxisLabels
                 {
                     Rotation = -45,
                     
                 }

             })
               .SetCredits(new Credits { Enabled = false })
               .SetSeries(new[]
                {
                      new Series {Name = "Days fall occurred",Color = ColorTranslator.FromHtml ("Red"), Id= " Hello" ,Data = new Data (didFall)},
                      new Series {Name = "Days no falls occurred",Color = ColorTranslator.FromHtml ("Green"), Id= " Hello" ,Data = new Data (notFall)}
                    //new Series { Name = "Falls ", Data = new Data(fallChart) }                   
                })
                .InFunction("DrawChart5");

            ////////////////////////////////////////////SLEEP PATTERN QUESTION  CHART  ///////////////////////////////
            var chart1 = new Highcharts("chart1")
            .InitChart(new Chart { Type = ChartTypes.Spline })
               .SetTitle(new Title { Text = "Sleep Patterns" })
                .SetCredits(new Credits { Enabled = false })
               .SetSubtitle(new Subtitle { Text = "Observation period from: " + dy[0] + " to: " + dy[dy.Length - 1] })
                     .SetXAxis(new XAxis
                {
                    Categories = dy,
                    Labels = new XAxisLabels
                    {
                        Rotation = -45,
                
                    }
                })
                .SetSeries(new Series
                {
                    Data = new Data(quest1),
                    Name = "Sleep Observation"
                })
                .SetYAxis(new YAxis
                {
                    Title = new YAxisTitle { Text = "Values" },
                    PlotBands = new[]
                    {
                        new YAxisPlotBands
                        {
                            From = 0.3,
                            To = 3.3,
                           Color = ColorTranslator.FromHtml("#ECF7FB"),
                            Label = new YAxisPlotBandsLabel
                            {
                                Text = "Unacceptable Level",
                                Style = "color:'#606060'"
                            }
                        },
                        new YAxisPlotBands
                        {
                            From = 3.3,
                            To = 5.5,
                            Color = Color.White,
                            Label = new YAxisPlotBandsLabel
                            {
                                Text = "Monitoring Required",
                                Style = "color:'#606060'"
                            }
                        },
                        new YAxisPlotBands
                        {
                            From = 5.5,
                            To = 8,
                            Color = ColorTranslator.FromHtml("#ECF7FB"),
                            Label = new YAxisPlotBandsLabel
                            {
                                Text = "Lower Acceptable Level",
                                Style = "color:'#606060'"
                            }
                         },
                         new YAxisPlotBands
                         {
                             From = 8,
                             To = 12,
                              Color = Color.White,
                            Label = new YAxisPlotBandsLabel
                            {
                                Text = "Upper Acceptable Level",
                                Style = "color:'#606060'"
                            }
                         },
                         new YAxisPlotBands
                         {
                             From = 12,
                             To = 15,
                              Color = ColorTranslator.FromHtml("#ECF7FB"),
                            Label = new YAxisPlotBandsLabel
                            {
                                Text = "No Immediate Action Required",
                                Style = "color:'#606060'"
                            }
                         }
                    }
                })
                .InFunction("DrawChart1");

            ////////////////////////////////////////////APPETITE LEVEL QUESTION  CHART  ///////////////////////////////

            var chart2 = new Highcharts("chart2")
           .InitChart(new Chart { Type = ChartTypes.Scatter })
              .SetTitle(new Title { Text = "Appetite Levels" })
               .SetCredits(new Credits { Enabled = false })
              .SetSubtitle(new Subtitle { Text = "Observation period from: " + dy[0] + " to: " + dy[dy.Length - 1] })
                    .SetXAxis(new XAxis
                    {
                        Categories = dy,
                        Labels = new XAxisLabels
                        {
                            Rotation = -45,
                            
                        }
                    })
               .SetSeries(new Series
               {
                   Data = new Data(quest2),
                   Name = "Appetite Levels"
               })
               .SetYAxis(new YAxis
               {
                   Title = new YAxisTitle { Text = "Values" },
                   PlotBands = new[]
                    {
                        new YAxisPlotBands
                        {
                            From = 0.5,
                            To = 2.5,
                           Color = ColorTranslator.FromHtml("#ECF7FB"),
                            Label = new YAxisPlotBandsLabel
                            {
                                Text = "Unacceptable Level",
                                Style = "color:'#606060'"
                            }
                        },
                        new YAxisPlotBands
                        {
                            From = 2.5,
                            To = 5,
                            Color = Color.White,
                            Label = new YAxisPlotBandsLabel
                            {
                                Text = "Monitoring Required",
                                Style = "color:'#606060'"
                            }
                        },
                        new YAxisPlotBands
                        {
                            From = 5,
                            To = 7,
                            Color = ColorTranslator.FromHtml("#ECF7FB"),
                            Label = new YAxisPlotBandsLabel
                            {
                                Text = "Lower Acceptable Level",
                                Style = "color:'#606060'"
                            }
                         },
                         new YAxisPlotBands
                         {
                             From = 7,
                             To = 10,
                              Color = Color.White,
                            Label = new YAxisPlotBandsLabel
                            {
                                Text = "Upper Acceptable Level",
                                Style = "color:'#606060'"
                            }
                         },
                         new YAxisPlotBands
                         {
                             From = 10,
                             To = 15,
                              Color = ColorTranslator.FromHtml("#ECF7FB"),
                            Label = new YAxisPlotBandsLabel
                            {
                                Text = "No Immediate Action Required",
                                Style = "color:'#606060'"
                            }
                         }
                    }
               })
              .InFunction("DrawChart2");

            ////////////////////////////////////////////AWARENESS QUESTION  CHART  ///////////////////////////////

            var chart3 = new Highcharts("chart3")
            .InitChart(new Chart { Type = ChartTypes.Spline })
               .SetTitle(new Title { Text = "Resident Awareness" })
                .SetCredits(new Credits { Enabled = false })
               .SetSubtitle(new Subtitle { Text = "Observation period from: " + dy[0] + " to: " + dy[dy.Length - 1] })
                     .SetXAxis(new XAxis
                     {
                         Categories = dy,
                         Labels = new XAxisLabels
                         {
                             Rotation = -45
                         }
                     })
                .SetSeries(new Series
                {
                    Data = new Data(quest3),
                    Name = "Resident Awareness"
                })
               .SetYAxis(new YAxis
               {
                   Title = new YAxisTitle { Text = "Values" },
                   PlotBands = new[]
                    {
                        new YAxisPlotBands
                        {
                            From = 0.5,
                            To = 3.5,
                           Color = ColorTranslator.FromHtml("#ECF7FB"),
                            Label = new YAxisPlotBandsLabel
                            {
                                Text = "Unacceptable Level",
                                Style = "color:'#606060'"
                            }
                        },
                        new YAxisPlotBands
                        {
                            From = 3.5,
                            To = 5.5,
                            Color = Color.White,
                            Label = new YAxisPlotBandsLabel
                            {
                                Text = "Monitoring Required",
                                Style = "color:'#606060'"
                            }
                        },
                        new YAxisPlotBands
                        {
                            From = 5.5,
                            To = 8,
                            Color = ColorTranslator.FromHtml("#ECF7FB"),
                            Label = new YAxisPlotBandsLabel
                            {
                                Text = "Lower Acceptable Level",
                                Style = "color:'#606060'"
                            }
                         },
                         new YAxisPlotBands
                         {
                             From = 8,
                             To = 11,
                              Color = Color.White,
                            Label = new YAxisPlotBandsLabel
                            {
                                Text = "Upper Acceptable Level",
                                Style = "color:'#606060'"
                            }
                         },
                         new YAxisPlotBands
                         {
                             From = 11,
                             To = 15,
                              Color = ColorTranslator.FromHtml("#ECF7FB"),
                            Label = new YAxisPlotBandsLabel
                            {
                                Text = "No Immediate Action Required",
                                Style = "color:'#606060'"
                            }
                         }
                    }
               })
                .InFunction("DrawChart3");

            ////////////////////////////////////////////RESTING POSITION QUESTION  CHART  ///////////////////////////////

            var chart4 = new Highcharts("chart4")
             .InitChart(new Chart { Type = ChartTypes.Scatter })
                .SetTitle(new Title { Text = "Resting Position" })
                 .SetCredits(new Credits { Enabled = false })
                .SetSubtitle(new Subtitle { Text = "Observation period from: " + dy[0] + " to: " + dy[dy.Length - 1] })
                      .SetXAxis(new XAxis
                      {
                          Categories = dy,
                          Labels = new XAxisLabels
                          {
                              Rotation = -45
                          }
                      })
                 .SetSeries(new Series
                 {
                     Data = new Data(quest4),
                     Name = "Resting Position"
                 })
                 .SetYAxis(new YAxis
                 {
                     Title = new YAxisTitle { Text = "Values" },
                     PlotBands = new[]
                    {
                        new YAxisPlotBands
                        {
                            From = 0.1,
                            To = 3,
                           Color = ColorTranslator.FromHtml("#ECF7FB"),
                            Label = new YAxisPlotBandsLabel
                            {
                                Text = "Unacceptable Level",
                                Style = "color:'#606060'"
                            }
                        },
                        new YAxisPlotBands
                        {
                            From = 3.0,
                            To = 4.5,
                            Color = Color.White,
                            Label = new YAxisPlotBandsLabel
                            {
                                Text = "Monitoring Required",
                                Style = "color:'#606060'"
                            }
                        },
                        new YAxisPlotBands
                        {
                            From = 4.5,
                            To = 6.5,
                            Color = ColorTranslator.FromHtml("#ECF7FB"),
                            Label = new YAxisPlotBandsLabel
                            {
                                Text = "Lower Acceptable Level",
                                Style = "color:'#606060'"
                            }
                         },
                         new YAxisPlotBands
                         {
                             From = 6.5,
                             To = 9.1,
                              Color = Color.White,
                            Label = new YAxisPlotBandsLabel
                            {
                                Text = "Upper Acceptable Level",
                                Style = "color:'#606060'"
                            }
                         },
                         new YAxisPlotBands
                         {
                             From = 9.1,
                             To = 12,
                              Color = ColorTranslator.FromHtml("#ECF7FB"),
                            Label = new YAxisPlotBandsLabel
                            {
                                Text = "No Immediate Action Required",
                                Style = "color:'#606060'"
                            }
                         }
                    }

                 })
                 .InFunction("DrawChart4");



            // traverse the array to find lowest & highest score

            var bestDay = numbers.Cast<int>().Concat(new[] {0}).Max();

            var worstDay = numbers.Cast<int>().Concat(new[] {1000}).Min();
            @ViewBag.worst = worstDay;
            @ViewBag.best = bestDay;

            // total observations passed to view
            @ViewBag.tot = n.Length;
            //return View(chart);
            return View(new Container(new[] { chart, chart1, chart2, chart5, chart4,chart3}));
        }
        public ActionResult Show()
        {
            return View();
        }
        // GET: Patients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var patient = _db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // GET: Patients/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PatientID,Name,MedicalCard,Gender,DoctorName,Wing")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                _db.Patients.Add(patient);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(patient);
        }

        // GET: Patients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var patient = _db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PatientID,Name,MedicalCard,Gender,DoctorName,Wing")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(patient).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(patient);
        }

        // GET: Patients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var patient = _db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var patient = _db.Patients.Find(id);
            _db.Patients.Remove(patient);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
