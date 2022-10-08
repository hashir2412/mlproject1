using Project1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    public class Knn
    {
        public List<PersonWithAge> personsWithAge;

        public async Task GenerateDataAndPredict()
        {
            await PredictData();
            Console.WriteLine("......................................");
            await PerformOneOutEvaluation(true);
            await PerformOneOutEvaluation(false);
        }

        async Task PerformOneOutEvaluation(bool useAgeInCalculation)
        {
            personsWithAge = new List<PersonWithAge>();
            string[] programData1c1d2c2d = await File.ReadAllLinesAsync("programData1c1d2c2d.txt");
            foreach (var item in programData1c1d2c2d)
            {
                var itemDetail = item.Replace("(", "").Replace(")", "").Split(",");
                var newItem = new PersonWithAge()
                {
                    Height = double.Parse(itemDetail[0].Trim()),
                    Weight = double.Parse(itemDetail[1].Trim()),
                    Age = int.Parse(itemDetail[2].Trim()),
                    Gender = itemDetail[3].Trim()
                };
                personsWithAge.Add(newItem);
            }
            for (int i = 0; i < personsWithAge.Count; i++)
            {
                var elementToBeTested = personsWithAge[i];
                personsWithAge.Remove(personsWithAge[i]);
                List<DistanceFromPoint> distanceFromPoints = new List<DistanceFromPoint>();
                foreach (var person in personsWithAge)
                {
                    var cartesianDistance = CalculateCartesianDistance(person, elementToBeTested, useAgeInCalculation);
                    distanceFromPoints.Add(new DistanceFromPoint()
                    {
                        Person = person,
                        CartesianDistance = cartesianDistance
                    });
                }
                var distanceSorted = distanceFromPoints.OrderBy(a => a.CartesianDistance).ToArray();
                FinalResult(distanceSorted, 1, elementToBeTested, "Cartesian");
                FinalResult(distanceSorted, 3, elementToBeTested, "Cartesian");
                FinalResult(distanceSorted, 5, elementToBeTested, "Cartesian");
                FinalResult(distanceSorted, 7, elementToBeTested, "Cartesian");
                FinalResult(distanceSorted, 9, elementToBeTested, "Cartesian");
                FinalResult(distanceSorted, 11, elementToBeTested, "Cartesian");
                Console.WriteLine($"{i} Actual Value: {elementToBeTested.Gender} , One out Evaluation with Age used: {useAgeInCalculation}");
                Console.WriteLine("------------------------------");
                personsWithAge.Add(elementToBeTested);
            }
        }

        async Task PredictData()
        {
            personsWithAge = new List<PersonWithAge>();
            string[] trainingtData1a2a = await File.ReadAllLinesAsync("trainingtData1a2a.txt");
            foreach (var item in trainingtData1a2a)
            {
                var itemDetail = item.Replace("(", "").Replace(")", "").Split(",");
                var newItem = new PersonWithAge()
                {
                    Height = double.Parse(itemDetail[0].Trim()),
                    Weight = double.Parse(itemDetail[1].Trim()),
                    Age = int.Parse(itemDetail[2].Trim()),
                    Gender = itemDetail[3].Trim()
                };
                personsWithAge.Add(newItem);
            }
            string[] testData1a2a = await File.ReadAllLinesAsync("testData1a2a.txt");
            foreach (var item in testData1a2a)
            {
                List<DistanceFromPoint> distanceFromPoints = new List<DistanceFromPoint>();
                var itemDetail = item.Replace("(", "").Replace(")", "").Split(",");
                var newItem = new PersonWithAge()
                {
                    Height = double.Parse(itemDetail[0].Trim()),
                    Weight = double.Parse(itemDetail[1].Trim()),
                    Age = int.Parse(itemDetail[2].Trim())
                };
                foreach (var person in personsWithAge)
                {
                    var manhattanDistance = CalculateManhattanDistance(person, newItem);
                    var minkowskiDistance = CalculateMinkowskiDistance(person, newItem);
                    var cartesianDistance = CalculateCartesianDistance(person, newItem);
                    distanceFromPoints.Add(new DistanceFromPoint()
                    {
                        ManhattanDistance = manhattanDistance,
                        Person = person,
                        MinkowskiDistance = minkowskiDistance,
                        CartesianDistance = cartesianDistance
                    });
                }
                var distanceSorted = distanceFromPoints.OrderBy(a => a.ManhattanDistance).ToArray();
                FinalResult(distanceSorted, 1, newItem, "Manhattan");
                FinalResult(distanceSorted, 3, newItem, "Manhattan");
                FinalResult(distanceSorted, 7, newItem, "Manhattan");
                distanceSorted = distanceFromPoints.OrderBy(a => a.MinkowskiDistance).ToArray();
                FinalResult(distanceSorted, 1, newItem, "Minkowski");
                FinalResult(distanceSorted, 3, newItem, "Minkowski");
                FinalResult(distanceSorted, 7, newItem, "Minkowski");
                distanceSorted = distanceFromPoints.OrderBy(a => a.CartesianDistance).ToArray();
                FinalResult(distanceSorted, 1, newItem, "Cartesian");
                FinalResult(distanceSorted, 3, newItem, "Cartesian");
                FinalResult(distanceSorted, 7, newItem, "Cartesian");
                Console.WriteLine("--------------------------------------");
                Console.WriteLine();
            }

        }

        void FinalResult(DistanceFromPoint[] distanceFromPoints, int k, PersonWithAge p1, string matrixUsed)
        {
            var Mcount = 0;
            var FCount = 0;
            for (int i = 0; i < k; i++)
            {
                if (distanceFromPoints[i].Person.Gender == "M")
                {
                    Mcount++;
                }
                else
                {
                    FCount++;
                }
            }
            if (Mcount > FCount)
            {
                Console.WriteLine($"1a) Gender Prediction for K = {k} for Person with {p1.Height}, {p1.Weight} and {p1.Age} using {matrixUsed} Distance is M");
            }
            else
            {
                Console.WriteLine($"1a) Gender Prediction for K = {k} for Person with {p1.Height}, {p1.Weight} and {p1.Age} using {matrixUsed} Distance is W");
            }
        }

        double CalculateManhattanDistance(PersonWithAge p1, PersonWithAge p2)
        {
            return Math.Abs(p1.Age - p2.Age) + Math.Abs(p1.Height - p2.Height) + Math.Abs(p1.Weight - p2.Weight);
        }

        double CalculateMinkowskiDistance(PersonWithAge p1, PersonWithAge p2)
        {
            double distance = 0;
            distance += Math.Pow(Math.Abs(p1.Height - p2.Height), 3);
            distance += Math.Pow(Math.Abs(p1.Weight - p2.Weight), 3);
            distance += Math.Pow(Math.Abs(p1.Age - p2.Age), 3);
            distance = Math.Pow(distance, (double)1 / 3);
            return distance;
        }

        double CalculateCartesianDistance(PersonWithAge p1, PersonWithAge p2, bool useAgeInCalculation = true)
        {
            double distance = 0;
            distance += Math.Pow(Math.Abs(p1.Height - p2.Height), 2);
            distance += Math.Pow(Math.Abs(p1.Weight - p2.Weight), 2);
            if (useAgeInCalculation)
            {
                distance += Math.Pow(Math.Abs(p1.Age - p2.Age), 2);
            }            
            distance = Math.Pow(distance, (double)1 / 2);
            return distance;
        }
    }
}

class DistanceFromPoint
{
    public Person Person { get; set; }
    public double ManhattanDistance { get; set; }

    public double MinkowskiDistance { get; set; }

    public double CartesianDistance { get; set; }
}
