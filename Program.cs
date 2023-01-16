using System;
using System.Collections.Generic;
namespace Elevator_Project
{
 
    class Program
    {

        static void Main(string[] args)
        {
            Elevator lift = new Elevator();
            List<Human> humen = new List<Human>();
            humen.Add(new Human(1, 5, 50, "Tom", lift));
            humen.Add(new Human(2, 8, 100, "Steve", lift));
            humen.Add(new Human(7, 9, 80, "Bob", lift));
            lift.Start(humen);
            humen.Add(new Human(5, 6, 90, "Jerry", lift));
            lift.Start(humen);


            Console.WriteLine("");
            lift.ShowFloor();
            lift.ShowTotalWeight();
            lift.ShowTotalSteps();
            lift.ShowTotalEmptySteps();
            Console.WriteLine($"Total passengers count is {humen.Count}");
            lift.ShowTotalPassengersDelivered();
        }
    }
}

class Elevator
{
    private static int n = 10;
    private static int maxweight = 400;
    private int floor = 5;
    private bool[] panel = new bool[n];
    private int weight = 0;
    private bool overload = false;
    private int status = 0;
    private int destination = 0;
    private int totalSteps = 0;
    private int totalWeight = 0;
    private int totalEmptySteps = 0;
    private int totalPassengersDelivered = 0;

    public void ShoWeight()
    {
        Console.WriteLine($"Elevator weight is {weight}");
    }
    public void ShowFloor()
    {
        Console.WriteLine($"Elevator is on {floor} floor");
    }
    public void PressPanelButton(int button)
    {
        panel[button] = true;
    }
    public void AddHuman(List<Human> humen)
    {
        status = 2;
        for (int i=0; i<humen.Count; i++)
        {
            if (humen[i].GetFloor() == floor && humen[i].GetStatus()==0)
            {

                if (weight+humen[i].GetWeight()<maxweight)
                {
                    totalWeight+= humen[i].GetWeight(); 
                    weight += humen[i].GetWeight();
                    Console.WriteLine($"Added new passenger {humen[i].GetName()} on {floor} floor, lift weight is {weight}");
                    PressPanelButton(humen[i].GetDestination());
                    humen[i].SetStatus(1);
                }
                else
                {
                    overload = true;
                    Console.WriteLine($"Lift is overloaded");
                    humen[i].SetStatus(2);
                    overload = false;
                }
            }
        }
        status = 1;
    }
    public void SetHumenFloor (List<Human> humen)
    {
        for (int i = 0; i < humen.Count; i++)
        {
            if (humen[i].GetStatus() == 1)
            {
                humen[i].SetFloor(floor);
            }
        }
    }
    public void DestinationReach(List<Human> humen)
    {
        for (int i = 0; i < humen.Count; i++)
        {
            if (humen[i].GetDestination() == floor && humen[i].GetStatus()==1)
            {
                weight -= humen[i].GetWeight();
                Console.WriteLine($"Human {humen[i].GetName()} delivered to floor {floor}, lift weight is {weight}");
                humen[i].SetStatus(2);
                totalPassengersDelivered++;
            }
        }
    }
    public void ShowStatus()
    {
        switch (status)
        {
            case 0:
                Console.WriteLine("Elevator is closed");
                break;
            case 1:
                Console.WriteLine("Elevator is open");
                break;
            case 2:
                Console.WriteLine("Elevator is moving");
                break;
        }
    }
    public void MoveUp(List<Human> humen)
    {
        status = 2;
        while(floor<destination)
        {
            SetHumenFloor(humen);
            floor++;
            if (weight == 0)
                totalEmptySteps++;
            totalSteps++;
            Console.WriteLine("");
            status = 2;
            ShowFloor();

            if (panel[floor]==true)
            {
                DestinationReach(humen);
                AddHuman(humen);
                panel[floor] = false;
            }
        }
    }
    public void MoveDown(List<Human> humen)
    {
        status = 2;
        while (floor > destination)
        {
            floor--;
            if (weight == 0)
                totalEmptySteps++;
            totalSteps++;
            Console.WriteLine("");
            SetHumenFloor(humen);
            status = 2;
            ShowFloor();
            if (panel[floor] == true)
            {
                DestinationReach(humen);
                AddHuman(humen);
                panel[floor] = false;
            }
         
        }
    }
    public void Start(List<Human> humen)
    {
        ShowFloor();
        do
        {
            ChangeDestination();
            if (floor < destination)
            {
                MoveUp(humen);
                ChangeDestination();
            }
            else
            {
                MoveDown(humen);
                ChangeDestination();
            }

        } while (destination != floor);
    }
    public void ChangeDestination()
    {
       bool check=CheckPanel();
        if (check)
        {
            for (int i = 0; i < n; i++)
            {
                if (floor > i && panel[i] == true)
                {
                    destination = i;
                    break;
                }
                if (floor < i && panel[i] == true)
                {
                    destination = i;
                    break;
                }
            }
        }
        else
            destination = floor;
    }
    public bool CheckPanel()
    {
        bool check = false;
        for (int i=0; i<n; i++)
        {
            if (panel[i]==true)
            {
                check = true;
                break;
            }
        }
        return check;
    }
    public void ShowTotalWeight()
    {
        Console.WriteLine($"Total weight moved is {totalWeight}");
    }
    public void ShowTotalSteps()
    {
        Console.WriteLine($"Total steps done is {totalSteps}");
    }
    public void ShowTotalEmptySteps()
    {
        Console.WriteLine($"Total steps without passengers is {totalEmptySteps}");
    }
    public void ShowTotalPassengersDelivered()
    {
        Console.WriteLine($"Total passengers delivered is {totalPassengersDelivered}");
    }
 
}

class Human
{
    private string name;
    private int floor = 0;
    private int status = 0;
    private int destination = 0;
    private int weight = 0;

   public Human(int a, int b, int c, string n, Elevator lift)
    {
        floor = a;
        destination = b;
        weight = c;
        lift.PressPanelButton(a);
        name = n;
    }
    public void ShowStatus()
    {
        switch (status)
        {
            case 0:
                Console.WriteLine($"Waiting on floor {floor}");
                break;
            case 1:
                Console.WriteLine($"Moving in elevator on floor {floor}");
                break;
            case 2:
                Console.WriteLine($"Delivered to floor {destination}");
                break;
        }
    }
    public int GetWeight()
    {
        return weight;

    }
    public void SetStatus(int s)
    {
        status = s;
    }
    public void SetFloor(int f)
    {
        floor = f;
    }
    public void SetDestination(int d)
    {
        destination = d;
    }
    public void SetWeight (int w)
    {
        weight = w;
    }
    public int GetFloor()
    {
        return floor;
    }
    public int GetDestination()
    {
        return destination;
    }
    public int GetStatus()
    {
        return status;
    }
    public string GetName()
    {
        return name;
    }

}