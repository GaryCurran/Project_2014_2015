using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShadyPines.Models;
using System.Web.Helpers;
using DotNet.Highcharts.Options;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Enums;
using System.Drawing;
using DotNet.Highcharts;

namespace ShadyPines.Controllers
{
    public class PatientsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private static int passedID = 0;
        // GET: Patients
        [Authorize]
        public ActionResult Index(string sarchString)
        // public ActionResult Index()
        {
            var res = from r in db.Patients select r;
            if (!String.IsNullOrEmpty(sarchString))
            {
                res = res.Where(s => s.Name.Contains(sarchString));
            }
            return View(res);
            // return View(db.Patients.ToList());
        }

        public ActionResult All()
        {
            return View(db.Patients.ToList());
        }

        [Authorize]
        public ActionResult Chart(int? id)
        {

            //Mine
            Patient pt = new Patient();

            MedicalQuestion mq = new MedicalQuestion();
            //mq = db.MedicalQuestions.Where(p => p.patientID == id).SingleOrDefault();
            if (id != null)
            {
                passedID = (int)id;
            }

            // Patients Details
            pt = db.Patients.Where(p => p.PatientID == passedID).SingleOrDefault();
            //pt = db.Patients.Where(p => p.PatientID == id).SingleOrDefault();
            ViewBag.name = pt.Name;
            ViewBag.a = pt.Gender;
            ViewBag.b = pt.MedicalCard;
            ViewBag.c = pt.DoctorName;

            // get all daily scores
            // from DB and transfer to array
            var tot = from e in db.MedicalQuestions
                      where e.patientID == id
                      select e;


            // get total amount of objects to set size of object arrays
            int size = tot.Count();

            string[] t = new string[size];
            int pos = 0;
            foreach (var it in tot)
            {
                t[pos] = it.Date.Date.ToShortDateString();
                pos++;
            }
            //DateTime date1 = new DateTime(2008, 6, 1, 7, 47, 0);
            //DateTime dateOnly = date1.Date;
            string[] dy = new string[size];
            for (int j = 0; j < size; j++)
            {

                dy[j] = t[j].ToString();

            }
            // Use total amount of daily reports
            // to size array
            // pass to chart as an object
            object[] numbers = new object[size];
            int i = 0;

            // holding the total daily scores
            foreach (var item in tot)
            {
                numbers[i] = item.DailyTotal;
                i++;
            }

            // Sleep Paterns for period
            object[] quest1 = new object[size];
            int count = 0;
            foreach (var item in tot)
            {
                quest1[count] = ((int)item.Question1 + 1) * 2;
                count++;
            }

            int topSleep = 0;



            for (int a = 0; a < size; a++)
            {
                if ((int)quest1[a] > topSleep)
                {
                    topSleep = (int)quest1[a];
                }
            }
            // calculate fall period
            int fall = 0;
            int fell = 0;
            int noFall = 0;
            object[] falls = new object[size];
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
            object[] fallChart = new object[size];
            int counter = 0;
            foreach (var item in tot)
            {
                if (item.HasFallen.Equals(true))
                {
                    fallChart[counter] = ((int)-1);
                    counter++;
                }
                else
                {
                    fallChart[counter] = ((int)1);
                    counter++;
                }
            }

            object[] fallChartSeverity = new object[size];

            for (int f = 0; f < fallChart.Length; f++)
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
            object[] didFall = new object[size];
            object[] notFall = new object[size];

            for (int b = 0; b < size; b++)
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
            object[] quest2 = new object[size];
            int count2 = 0;
            foreach (var item in tot)
            {
                quest2[count2] = ((int)item.Question2 + 1) * 2;
                count2++;
            }

            // Appetite for period
            object[] quest3 = new object[size];
            int count3 = 0;
            foreach (var item in tot)
            {
                quest3[count3] = ((int)item.Question3 + 1) * 2;
                count3++;
            }

            // resting position
            object[] quest4 = new object[size];
            int count4 = 0;
            foreach (var item in tot)
            {
                quest4[count4] = ((int)item.Question4 + 1) * 2;
                count4++;
            }
            // length of stay determined by size of array
            string[] days = new string[size];

            // display No. of days on X axis
            for (int j = 0; j < days.Length; j++)
            {
                days[j] = "Day " + (j + 1);
            }

            // create another array to run comparissions
            object[] n = numbers;

            // sum total daily scores
            int s = 0;
            for (int k = 0; k < numbers.Length; k++)
            {
                s += (int)n[k];
            }

            // get avg daily score
            @ViewBag.avg = s / n.Length;

            // avg daily score to pass to graph
            object[] avgerageScore = new object[size];

            // populate avgerageScore []
            for (int g = 0; g < days.Length; g++)
            {
                avgerageScore[g] = @ViewBag.avg;
            }
            // This is a testing comment!!!!!!!!!!!
            // total number of daily observations
            ViewBag.total = numbers;

            // total number of days in hospital
            int d = days.Count();

            ////////////////////////////////////////////SUMMARY   CHART  ///////////////////////////////
            DotNet.Highcharts.Highcharts chart = new DotNet.Highcharts.Highcharts("chart")
             .InitChart(new DotNet.Highcharts.Options.Chart
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
                    .SetLegend(new Legend
                    {
                        
                    })
                        .SetLabels(new Labels
                        {

                        })
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
                            new DotNet.Highcharts.Options.Point
                            {
                                Name = "Days falls occured",
                                Y = fell,
                                Color = Color.FromName("Red")
                            },
                            new DotNet.Highcharts.Options.Point
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
            Highcharts chart5 = new Highcharts("chart5")
               .InitChart(new DotNet.Highcharts.Options.Chart { Type = ChartTypes.Column, 
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
                .InFunction("DrawChart5"); ;

            ////////////////////////////////////////////SLEEP PATTERN QUESTION  CHART  ///////////////////////////////
            Highcharts chart1 = new Highcharts("chart1")
            .InitChart(new DotNet.Highcharts.Options.Chart { Type = ChartTypes.Spline })
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

            Highcharts chart2 = new Highcharts("chart2")
           .InitChart(new DotNet.Highcharts.Options.Chart { Type = ChartTypes.Scatter })
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

            Highcharts chart3 = new Highcharts("chart3")
            .InitChart(new DotNet.Highcharts.Options.Chart { Type = ChartTypes.Spline })
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

            Highcharts chart4 = new Highcharts("chart4")
             .InitChart(new DotNet.Highcharts.Options.Chart { Type = ChartTypes.Scatter })
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

            int bestDay = 0;
            int worstDay = 1000;
            for (int j = 0; j < numbers.Length; j++)
            {
                if ((int)numbers[j] > bestDay)
                {
                    bestDay = (int)numbers[j];
                }
            }

            for (int j = 0; j < numbers.Length; j++)
            {
                if ((int)numbers[j] < worstDay)
                {
                    worstDay = (int)numbers[j];
                }
            }
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
            Patient patient = db.Patients.Find(id);
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
                db.Patients.Add(patient);
                db.SaveChanges();
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
            Patient patient = db.Patients.Find(id);
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
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
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
            Patient patient = db.Patients.Find(id);
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
            Patient patient = db.Patients.Find(id);
            db.Patients.Remove(patient);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
