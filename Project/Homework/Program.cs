using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {


        static void Main(string[] args)
        {
            //Total Lines declaration
            int totalLinesGps;
            int totalLinesNrRoute;
            int totalLinesRoute;
            int counter = 0;
            int firstStop = 0;
            int lastStop = 0;
            int i = 0;

            //Strings
            String routescontent;
            String busNumber;
            String busId;

            //Big lists
            List<string> busList = new List<string>();
            List<string> stopList = new List<string>();

            //Splitted lists of gps.txt
            List<string> vechicleType = new List<string>();
            List<string> vechicleNr = new List<string>();
            List<string> vechicleLng = new List<string>();
            List<string> vechicleLat = new List<string>();
            List<string> mFromStop = new List<string>();
            List<string> vechicleId = new List<string>();

            //Splitted lists of routes.txt
            List<string> vechicleRoute = new List<string>();
            List<string> vechicleNrRoute = new List<string>();

            //Splitted lists of stops.txt
            List<string> vechicleStopId = new List<string>();
            List<string> vechicleStopsRoute = new List<string>();
            List<string> vechicleStopName = new List<string>();
            List<string> vechicleStopLng = new List<string>();
            List<string> vechicleStopLat = new List<string>();
            List<string> search = new List<string>();

            //Bus stops name for a bus
            List<string> busStopNames = new List<string>();

            //Boolean to check if input is correct
            Boolean foundString = false;

            //Streams
            Stream gpsStream;
            Stream routesStream;
            Stream stopsStream;

            //Stream readers
            StreamReader gpsStreamReader;
            StreamReader routesStreamReader;
            StreamReader stopsStreamReader;

            WebClient client = new WebClient();
            gpsStream = client.OpenRead("http://soiduplaan.tallinn.ee/gps.txt");

            // Making bus list    
            using (gpsStreamReader = new StreamReader(gpsStream))
            {
                string line;
                while ((line = gpsStreamReader.ReadLine()) != null)
                {
                    busList.Add(line); // Add to list.
                }
            }

            //Gets how many line there are to determien bus count
            totalLinesGps = busList.Count;

            //Splitting busList to smaller lists
            foreach (string line in busList)
            {
                string[] splittedBusList = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string splittedBusListLine in splittedBusList)
                {
                    if (i == 0)
                    {
                        vechicleType.Add(splittedBusListLine);
                        i++;
                    }
                    else if (i == 1)
                    {
                        vechicleNr.Add(splittedBusListLine);
                        i++;
                    }
                    else if (i == 2)
                    {
                        vechicleLng.Add(splittedBusListLine);
                        i++;
                    }
                    else if (i == 3)
                    {
                        vechicleLat.Add(splittedBusListLine);
                        i++;
                    }
                    else if (i == 4)
                    {
                        mFromStop.Add(splittedBusListLine);
                        i++;
                    }
                    else if (i == 5)
                    {
                        vechicleId.Add(splittedBusListLine);
                        i = 0;
                    }

                }

            }

            routesStream = client.OpenRead("http://soiduplaan.tallinn.ee/data/routes.txt");
            routesStreamReader = new StreamReader(routesStream);
            routescontent = routesStreamReader.ReadToEnd();

            //Splitting to get vechicle route and vechicle number
            char[] delimiter = new char[] { ';', '\n', };
            string[] routesSplitArray = routescontent.Split(delimiter);
            i = 0;
            foreach (string entry in routesSplitArray)
            {
                if (i == 0)
                {
                    vechicleNrRoute.Add(entry);
                }
                if (i == 13)
                {
                    vechicleRoute.Add(entry);
                }
                else if (i == 15)
                {
                    i = -1;
                }
                i++;
            }

            //Removing first line which is description
            vechicleNrRoute.RemoveRange(0, 1);
            vechicleRoute.RemoveRange(0, 1);

            //Counting totalLines
            totalLinesNrRoute = vechicleNrRoute.Count;
            totalLinesRoute = vechicleRoute.Count;

            //Removing range which is unwanted
            vechicleNrRoute.RemoveRange(414, totalLinesNrRoute - 415);
            vechicleRoute.RemoveRange(414, totalLinesRoute - 415);

            stopsStream = client.OpenRead("http://soiduplaan.tallinn.ee/data/stops.txt");

            //Making stop list
            using (stopsStreamReader = new StreamReader(stopsStream))
            {
                string line;
                while ((line = stopsStreamReader.ReadLine()) != null)
                {
                    stopList.Add(line); // Add to list.
                }
            }

            //Splitting busList to smaller lists
            foreach (string line in stopList)
            {
                string[] splittedStopList = line.Split(new char[] { ';' });
                i = 0;
                foreach (string splittedStopListLine in splittedStopList)
                {
                    if (i == 0)
                    {
                        vechicleStopId.Add(splittedStopListLine);
                    }
                    else if (i == 2)
                    {
                        vechicleStopLat.Add(splittedStopListLine);
                    }
                    else if (i == 3)
                    {
                        vechicleStopLng.Add(splittedStopListLine);
                    }
                    else if (i == 4)
                    {
                        vechicleStopsRoute.Add(splittedStopListLine);
                    }
                    else if (i == 5)
                    {
                        vechicleStopName.Add(splittedStopListLine);
                    }
                    else if (i == 8)
                    {
                        i = -1;
                    }
                    i++;

                }

            }
            //There is no need to delete first line of these Lists, which is description.

            //User input
            Console.WriteLine("Enter bus ID:");
            busId = Console.ReadLine();

            //Making sure that inserted bus Id exists in gps.txt. Otherwise ask again.
            while (!foundString)
            {
                for (int j = 0; j < vechicleNr.Count; j++)
                {
                    if (busId == vechicleId[j])
                    {
                        foundString = true;
                    }
                }
                if (!foundString)
                {
                    Console.WriteLine("No such bus Id in gps.txt. Enter bus ID again: ");
                    busId = Console.ReadLine();
                }
               

            }
            
            //Making sure that inserted bus number exists in gps.txt. Otherwise ask again.
            Console.WriteLine("Enter bus number:");
            busNumber = Console.ReadLine();
            foundString = false;

            while (!foundString)
            {
                for (int j = 0; j < vechicleNr.Count; j++)
                {
                    if (busNumber == vechicleNr[j])
                    {
                        foundString = true;
                    }
                }
                if (!foundString)
                {
                    Console.WriteLine("No such bus number in gps.txt. Enter bus ID again: ");
                    busNumber = Console.ReadLine();
                }


            }

            //Create the file. 
            using (FileStream fs = File.Create("new_gps.txt"))
            {
                //Can search from every splitted lists and change output. 
                //Could be used with inputs and outputs: vechicle Latitude, Longitude, Id, Number, m From next stop , depending on need.

                //Example 1
                AddText(fs, "#Example nr 1 of output depending on input.Input bus id.");
                for (int j = 0; j < vechicleId.Count; j++)
                {
                    if (vechicleId[j] == busId)
                    {
                        AddText(fs, "\nVechileLongitude: " + vechicleLng[j] + " Vechicle Latitude: " + vechicleLat[j] + " VechicleId: " + vechicleId[j] + " Vechicle number:" + vechicleNr[j]);
                    }

                }

                //Example 2
                AddText(fs, "\n\n#Example nr 2 of output depending on input. Input bus number.");
                for (int j = 0; j < vechicleNr.Count; j++)
                {
                    if (vechicleNr[j] == busNumber)
                    {
                        AddText(fs, "\nVechile Longitude: " + vechicleLng[j] + " Vechicle Latitude: " + vechicleLat[j] + " Vechicle Number: " + vechicleNr[j] + " Vechicle Id: " + vechicleId[j]);
                    }

                }

                //Coulde be used to print out lists of stops.txt(Long list thats why not included in gps_new.txt)
                /* for (int j = 0; j < vechicleStopName.Count; j++)
                 {
                     AddText(fs, "\nVechile Stop Lng: " + vechicleStopLng[j] + " Vechicle stop Lat: " + vechicleStopLat[j] + " Vechicle stop id: " + vechicleStopId[j] + " Vechicle stop name: " + vechicleStopName[j]);

                 }*/

                //Could be used to print out lists of gps.txt
                /* for (int j = 0; j < vechicleNr.Count; j++)
                 {
                     if (vechicleNr[j] == busNumber)
                     {
                         AddText(fs, "\nVechicle Id: " + vechicleId[j] + " Vechicle Number: " + vechicleNr[j]);
                     }

                 }*/

                //Outputs how many busses are using Gps at the moment
                AddText(fs, "\n\nHow many busses use Gps:" + totalLinesGps);

                //Could be used with inputs of: vechicle Latitude, Longitude, Id, Number, m From next stop , depending on need.Also output can be of stops.txts or routes.txt parameters(ID;SiriID;Lat;Lng;Stops).

                //Example:
                AddText(fs, "\n\n#Example of adding information to gps.txt from stops.txt");
                AddText(fs, "\nVechicleType;VechicleNumber;VechicleLongitude;VechicleLatitude;VechicleMFromStop;VechicleId"+
                    ";VechicleFirstStopId;VechicleFirstStopLongitude;VechicleFirstStopLatitude;VechicleLastStopId;VechicleLastStopLongitude;VechicleLastStopLatitude");
                for (int m = 0; m < vechicleNr.Count; m++)
                {
                    if (vechicleNr[m] == "0")
                    {
                        AddText(fs, "\n" + vechicleType[m] + ";" + vechicleNr[m] + ";" + vechicleLng[m] + ";" + vechicleLat[m] + ";" + mFromStop[m] + ";" + 
                            vechicleId[m] + ";" + "Depoo" + ";" + vechicleStopLng[firstStop] + ";" + ";" + "Depoo" + ";" + ";");
                    }
                    for (int j = 0; j < vechicleNrRoute.Count; j++)
                    {
                        if (vechicleNrRoute[j] == vechicleNr[m])
                        {
                            //Splitting route to route-id-s
                            search = vechicleRoute[j].Split(',').Select(p => p.Trim()).ToList();

                            for (int l = 0; l < vechicleStopId.Count; l++)
                            {
                                if (search.First() == vechicleStopId[l])
                                {
                                    firstStop = l;
                                    counter++;

                                }
                                if (search.Last() == vechicleStopId[l])
                                {
                                    lastStop = l;
                                    counter++;

                                }
                                if (counter == 2)
                                {
                                    counter = 0;
                                    AddText(fs, "\n" + vechicleType[m] + ";" + vechicleNr[m] + ";" + vechicleLng[m] + ";" + vechicleLat[m] + ";" + mFromStop[m] + ";" + vechicleId[m] + ";" + search.First() +
                                        ";" + vechicleStopLng[firstStop] + ";" + vechicleStopLat[lastStop] + ";" + search.Last() + ";" + vechicleStopLng[lastStop] + ";" + vechicleStopLat[lastStop]);
                                    
                                }
                            }

                        }
                    }


                }
            }
        }

        //Function to add text to file
        private static void AddText(FileStream fs, string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        }
    }
}
