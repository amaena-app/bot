
using System.Text.Json;
using BaseEvents;

class Bot
{



    static void Main(String[] args)
    {
        EventBriteRequest request;
        using (StreamReader file = new StreamReader("request.json"))
        {
            request = JsonSerializer.Deserialize<EventBriteRequest>(file.ReadToEnd());
            file.Close();
        }

        EventBriteResponseList erl = new EventBriteResponseList();

        int page_count = 1;
        int page = 1;

        List<Task<EventBriteResponseList>> EBtasks = new();

        List<Event> totalEvents = [];




        do
        {

            //Console.WriteLine($"Pages: {erl.events.pagination.page_count}, current: {page}");
            request.event_search.page = page++;
            string request_string = JsonSerializer.Serialize(request);
            Console.WriteLine(request_string);
            Task<EventBriteResponseList> t = Task.Run(() => EventBrite.Richiedi(request_string));
            if(page == 2){
                t.Wait();
                erl = t.Result;
                Console.WriteLine(erl);
                page_count = erl.events.pagination.page_count;
            }else
                EBtasks.Add(t);
            

        } while (page_count >= page);

        Task.WaitAll(EBtasks.ToArray());

        foreach(Task<EventBriteResponseList> t in EBtasks){
            //Console.WriteLine(t.Result.ToString());
            erl += t.Result;
        }

        foreach(Event i in erl.Convert()){
            totalEvents.Add(i);
        }

        //Console.WriteLine($"{erl.events.pagination.object_count}, {erl.events.results.Length}");

        //Console.WriteLine(erl.ToString()); 

        

        string[] oggiromaTypes = ["mostre", "festival", "bandi-e-concorsi", "spettacoli", "bambini-e-famiglie"];

        List<Task<Event[]>> requestList = [];

        foreach (string tipo in oggiromaTypes)
        {
            requestList.Add(OggiRoma.Richiedi(tipo));
        }

        Task.WaitAll(requestList.ToArray());

        foreach (Task<Event[]> t in requestList)
        {

            foreach (Event e in t.Result)
            {
                totalEvents.Add(e);
            }

        }

        Task<Event[]> ZeroEvents = Zero.Richiedi();

        ZeroEvents.Wait();

        foreach(Event e in ZeroEvents.Result){
            totalEvents.Add(e);
        }

        using (StreamWriter file = new StreamWriter("events.json"))
        {
            file.WriteLine(JsonSerializer.Serialize(totalEvents));
            file.Flush();
            file.Close();
        }

        //Console.WriteLine(req.Result);

    }
}

