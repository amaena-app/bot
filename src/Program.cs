﻿
using System.Text.Json;
using BaseEvents;

class Bot
{



    static void Main(String[] args)
    {
        Console.WriteLine("------------------------------------------------------------");
        Console.WriteLine("Amaena Web Scraper");
        Console.WriteLine("------------------------------------------------------------");
        Console.WriteLine($"Current date and time: {DateTime.Now.ToString()}");
        Console.WriteLine("------------------------------------------------------------");

        string ebString = "";

        using (StreamReader file = new StreamReader("request.json"))
        {

            ebString = file.ReadToEnd();
            file.Close();
        }

        Dictionary<string, Dictionary<string,  Object>> dict = [];
        long totalEventCount = 0;


        foreach (City i in Cities.list)
        {

            long cityEventCount = 0;

            Console.WriteLine($"City: {i.nomeEng} ({i.lat}, {i.lon})");

            EventBriteRequest ebRequest = JsonSerializer.Deserialize<EventBriteRequest>(ebString);
            //ebRequest.event_search.date_range.from = DateTime.Now.ToString("yyyy-MM-dd");
            //ebRequest.event_search.date_range.to = DateTime.Now.AddDays(32).ToString("yyyy-MM-dd");
            ebRequest.event_search.places = [i.eventBriteCode];


            EventBriteResponseList erl = new EventBriteResponseList();

            int page_count = 1;
            int page = 1;

            List<Task<EventBriteResponseList>> EBtasks = new();

            List<Event> totalEvents = [];

            do
            {

                //Console.WriteLine($"Pages: {erl.events.pagination.page_count}, current: {page}");
                ebRequest.event_search.page = page++;
                string request_string = JsonSerializer.Serialize(ebRequest);

                if (Tools.VERBOSE)
                    Console.WriteLine(request_string);

                Task<EventBriteResponseList> t = Task.Run(() => EventBrite.Richiedi(request_string, i));
                if (page == 2)
                {
                    t.Wait();
                    erl = t.Result;

                    if (Tools.VERBOSE)
                        Console.WriteLine(erl);

                    page_count = erl.events.pagination.page_count;
                }
                else
                    EBtasks.Add(t);


            } while (page_count >= page);


            Task<Event[]> ZeroEvents = Zero.Richiedi(i);

            Task<Event[]> EventimEvents = Eventim.Richiedi(i);


            List<Task<Event[]>> customs = [];

            foreach (CustomRequests cr in i.requests)
            {
                customs.AddRange(cr.Richiedi());
            }

            Task[] tasks = [.. EBtasks];

            tasks = [.. tasks, ZeroEvents,EventimEvents, .. customs];


            Task.WaitAll(tasks);

            foreach (Task<EventBriteResponseList> t in EBtasks)
            {
                //Console.WriteLine(t.Result.ToString());
                erl += t.Result;
            }

            Event[] erlConverted = erl.Convert();

            Console.WriteLine($"EventBrite event count: {erlConverted.Length}");

            foreach (Event e in erlConverted)
            {
                totalEvents.Add(e);
            }

            Console.WriteLine($"Zero.eu event count: {ZeroEvents.Result.Length}");


            foreach (Event e in ZeroEvents.Result)
            {
                totalEvents.Add(e);
            }

            Console.WriteLine($"Eventim event count: {EventimEvents.Result.Length}");

            foreach (Event e in EventimEvents.Result)
            {
                totalEvents.Add(e);
            }

            Console.WriteLine($"Custom Requests ({customs.Count}):");

            foreach (Task<Event[]> t in customs)
            {
                Console.WriteLine($"    Custom {t.GetType().Name} event count: {t.Result.Length}");

                foreach (Event e in t.Result)
                {
                    totalEvents.Add(e);
                }

            }


            Dictionary<string, Object> cityDict = new();

            cityDict["lat"] = i.lat;
            cityDict["lon"] = i.lon;
            cityDict["events"] = totalEvents;

            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            foreach (Event e in totalEvents)
            {
                int start = Math.Clamp(Math.Max(e.from_date.DayNumber, today.DayNumber), 1, e.to_date.DayNumber);
                int stop = Math.Clamp(Math.Min(e.to_date.DayNumber, today.DayNumber + 31), start, e.to_date.DayNumber);
                if(Tools.VERBOSE)
                    Console.WriteLine($"{e.name} ({e.from_date} - {e.to_date} = {stop - start + 1} days)");   
                cityEventCount += stop - start + 1;
            }

            totalEventCount += cityEventCount;

            cityDict["event_count"] = cityEventCount;
            
            dict[i.nomeIta] = cityDict;

            Console.WriteLine($"Total events for {i.nomeEng}: {cityEventCount}");
            Console.WriteLine("------------------------------------------------------------");


        }

        Console.WriteLine($"Total events: {totalEventCount}");
        Console.WriteLine("------------------------------------------------------------");

        //Console.WriteLine($"{erl.events.pagination.object_count}, {erl.events.results.Length}");

        //Console.WriteLine(erl.ToString()); 

        Console.WriteLine("Writing events.json...");

        using (StreamWriter file = new StreamWriter("events.json"))
        {
            file.WriteLine(JsonSerializer.Serialize(dict));
            file.Flush();
            file.Close();
        }

        Console.WriteLine("Writing Complete");
        Console.WriteLine("------------------------------------------------------------");

        //Console.WriteLine(req.Result);

    }
}

