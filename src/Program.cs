
using System.Text.Json;
using BaseEvents;

class Bot
{



    static void Main(String[] args)
    {2
        string ebString = "";

        using (StreamReader file = new StreamReader("request.json"))
        {

            ebString = file.ReadToEnd();
            file.Close();
        }

        Dictionary<string, Event[]> dict = [];


        foreach (City i in Cities.list)
        {
            EventBriteRequest ebRequest = JsonSerializer.Deserialize<EventBriteRequest>(ebString);
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
                Console.WriteLine(request_string);
                Task<EventBriteResponseList> t = Task.Run(() => EventBrite.Richiedi(request_string, i));
                if (page == 2)
                {
                    t.Wait();
                    erl = t.Result;
                    Console.WriteLine(erl);
                    page_count = erl.events.pagination.page_count;
                }
                else
                    EBtasks.Add(t);


            } while (page_count >= page);


            Task<Event[]> ZeroEvents = Zero.Richiedi(i);


            List<Task<Event[]>> customs = [];

            foreach (CustomRequests cr in i.requests)
            {
                customs.AddRange(cr.Richiedi());
            }

            Task[] tasks = [.. EBtasks];

            tasks = [.. tasks, .. customs, ZeroEvents];


            Task.WaitAll(tasks);

            foreach (Task<EventBriteResponseList> t in EBtasks)
            {
                //Console.WriteLine(t.Result.ToString());
                erl += t.Result;
            }

            foreach (Event e in erl.Convert())
            {
                totalEvents.Add(e);
            }


            foreach (Event e in ZeroEvents.Result)
            {
                totalEvents.Add(e);
            }

            foreach (Task<Event[]> t in customs)
            {

                foreach (Event e in t.Result)
                {
                    totalEvents.Add(e);
                }

            }

            dict[i.nomeIta] = [.. totalEvents];


        }

        //Console.WriteLine($"{erl.events.pagination.object_count}, {erl.events.results.Length}");

        //Console.WriteLine(erl.ToString()); 



        using (StreamWriter file = new StreamWriter("events.json"))
        {
            file.WriteLine(JsonSerializer.Serialize(dict));
            file.Flush();
            file.Close();
        }

        //Console.WriteLine(req.Result);

    }
}

