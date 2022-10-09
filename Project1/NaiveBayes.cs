
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Project1
{
    internal class NaiveBayes
    {
        public List<PersonWithAge> personsWithAge;
        public async Task GenerateDataAndPredict()
        {
            await PredictData();
            Console.WriteLine("X------------------------X--------------------------X");
            await PerformOneOutEvaluation(true);
            await PerformOneOutEvaluation(false);
        }

        async Task PerformOneOutEvaluation(bool useAgeInCalculation)
        {
            personsWithAge = new List<PersonWithAge>();
            string[] programData1c1d2c2d = await File.ReadAllLinesAsync("programData1c1d2c2d.txt");
            double maleCount = 0;
            double femaleCount = 0;
            double heightWithMaleTotal = 0;
            double heightWithFemaleTotal = 0;
            double weightWithMaleTotal = 0;
            double weightWithFemaleTotal = 0;
            double ageWithMaleTotal = 0;
            double ageWithFemaleTotal = 0;

            int correctPrediction = 0;
            int falsePrediction = 0;
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
                //personsWithAge.RemoveAt(i);
                double heightWithMaleMean = heightWithMaleTotal / maleCount;
                double heightWithMale_SD = 0;
                double heightWithFemaleMean = heightWithFemaleTotal / femaleCount;
                double heightWithFemale_SD = 0;

                double weightWithMaleMean = weightWithMaleTotal / maleCount;
                double weightWithMale_SD = 0;
                double weightWithFemaleMean = weightWithFemaleTotal / femaleCount;
                double weightWithFemale_SD = 0;


                double ageWithMaleMean = ageWithMaleTotal / maleCount;
                double ageWithMale_SD = 0;
                double ageWithFemaleMean = ageWithFemaleTotal / femaleCount;
                double ageWithFemale_SD = 0;
                for(int j = 0; j < personsWithAge.Count; j++)
                {
                    if(i == j)
                    {
                        continue;
                    }
                    var newItem = personsWithAge[j];
                    if (newItem.Gender == "M")
                    {
                        maleCount++;
                        heightWithMaleTotal += newItem.Height;
                        weightWithMaleTotal += newItem.Weight;
                        ageWithMaleTotal += newItem.Age;
                    }
                    else
                    {
                        femaleCount++;
                        heightWithFemaleTotal += newItem.Height;
                        weightWithFemaleTotal += newItem.Weight;
                        ageWithFemaleTotal += newItem.Age;
                    }
                }
                for (int j = 0; j < personsWithAge.Count; j++)
                {
                    if(i == j)
                    {
                        continue;
                    }
                    var item = personsWithAge[j];
                    if (item.Gender == "M")
                    {
                        heightWithMale_SD += Math.Pow(Math.Pow(item.Height - heightWithMaleMean, 2) / (maleCount - 1), 0.5); ;
                        weightWithMale_SD += Math.Pow(Math.Pow(item.Weight - weightWithMaleMean, 2) / (maleCount - 1), 0.5);
                        ageWithMale_SD += Math.Pow(Math.Pow(item.Age - ageWithMaleMean, 2) / (maleCount - 1), 0.5);
                    }
                    else
                    {
                        heightWithFemale_SD += Math.Pow(Math.Pow(item.Height - heightWithFemaleMean, 2) / (femaleCount - 1), 0.5);
                        weightWithFemale_SD += Math.Pow(Math.Pow(item.Weight - weightWithFemaleMean, 2) / (femaleCount - 1), 0.5);
                        ageWithFemale_SD += Math.Pow(Math.Pow(item.Age - ageWithFemaleMean, 2) / (femaleCount - 1), 0.5);
                    }
                }

                double probabilityOfHeightGivenMale = (1 / (Math.Pow(2 * Math.PI, 0.5) * heightWithMale_SD)) * Math.Pow(Math.E, -Math.Pow(elementToBeTested.Height - heightWithMaleMean, 2) / Math.Pow(2 * heightWithMale_SD, 2));
                double probabilityOfWeightGivenMale = (1 / (Math.Pow(2 * Math.PI, 0.5) * weightWithMale_SD)) * Math.Pow(Math.E, -Math.Pow(elementToBeTested.Weight - weightWithMaleMean, 2) / Math.Pow(2 * weightWithMale_SD, 2));
                double probabilityOfAgeGivenMale = 1 / (Math.Pow(2 * Math.PI, 0.5) * ageWithMale_SD) * Math.Pow(Math.E, -Math.Pow(elementToBeTested.Age - ageWithMaleMean, 2) / Math.Pow(2 * ageWithMale_SD, 2));

                double probabilityOfHeightGivenFemale = (1 / (Math.Pow(2 * Math.PI, 0.5) * heightWithFemale_SD)) * Math.Pow(Math.E, -Math.Pow(elementToBeTested.Height - heightWithFemaleMean, 2) / Math.Pow(2 * heightWithFemale_SD, 2));
                double probabilityOfWeightGivenFemale = (1 / (Math.Pow(2 * Math.PI, 0.5) * weightWithFemale_SD)) * Math.Pow(Math.E, -Math.Pow(elementToBeTested.Weight - weightWithFemaleMean, 2) / Math.Pow(2 * weightWithFemale_SD, 2));
                double probabilityOfAgeGivenFemale = 1 / (Math.Pow(2 * Math.PI, 0.5) * ageWithFemale_SD) * Math.Pow(Math.E, -Math.Pow(elementToBeTested.Age - ageWithFemaleMean, 2) / Math.Pow(2 * ageWithFemale_SD, 2));

                double probabilityOfMale = maleCount / personsWithAge.Count;
                double probabilityOfFemale = femaleCount / personsWithAge.Count;
                double probabilityMaleTotal;
                double probabilityFemaleTotal;
                if (useAgeInCalculation)
                {
                    probabilityMaleTotal = probabilityOfHeightGivenMale * probabilityOfWeightGivenMale *
                    probabilityOfAgeGivenMale * probabilityOfMale;
                    probabilityFemaleTotal = probabilityOfHeightGivenFemale * probabilityOfWeightGivenFemale *
                    probabilityOfAgeGivenFemale * probabilityOfFemale;
                }
                else
                {
                    probabilityMaleTotal = probabilityOfHeightGivenMale * probabilityOfWeightGivenMale *
                    probabilityOfMale;
                    probabilityFemaleTotal = probabilityOfHeightGivenFemale * probabilityOfWeightGivenFemale *
                     probabilityOfFemale;
                }

                double finalTotalMale = probabilityMaleTotal / (probabilityMaleTotal + probabilityFemaleTotal);
                double finalTotalFemale = probabilityFemaleTotal / (probabilityMaleTotal + probabilityFemaleTotal);
                //Console.WriteLine($"{finalTotalMale}//{finalTotalFemale}");

                if (finalTotalMale > finalTotalFemale)
                {
                    //Console.WriteLine($"Naive Bayes {i} 1a) Gender Prediction for Person with {elementToBeTested.Gender} using Gaussian Naive Bayes is M");
                    //Console.WriteLine($"One out Evaluation with Age used: {useAgeInCalculation}");
                    if (elementToBeTested.Gender == "M")
                    {
                        correctPrediction++;
                    }
                    else
                    {
                        falsePrediction++;
                    }
                }
                else
                {
                    //Console.WriteLine($"Naive Bayes {i} 1a) Gender Prediction for Person with {elementToBeTested.Gender} using Gaussian Naive Bayes is W");
                    //Console.WriteLine($"One out Evaluation with Age used: {useAgeInCalculation}");
                    if (elementToBeTested.Gender == "W")
                    {
                        correctPrediction++;
                    }
                    else
                    {
                        falsePrediction++;
                    }
                }
            }
            Console.WriteLine($"Naive Bayes Correct: {correctPrediction} False {falsePrediction} Total Predictions{correctPrediction + falsePrediction} Age used {useAgeInCalculation}");
            Console.WriteLine("XXXXXXXXXXXXXXXXXXXXXXXXXXX-XXXXXXXXXXXXXXXXXXXXXXXXXX-XXXXXXXXXXXX");
        }

        async Task PredictData()
        {
            personsWithAge = new List<PersonWithAge>();
            string[] trainingtData1a2a = await File.ReadAllLinesAsync("trainingtData1a2a.txt");
            double maleCount = 0;
            double femaleCount = 0;
            double heightWithMaleTotal = 0;
            double heightWithFemaleTotal = 0;
            double weightWithMaleTotal = 0;
            double weightWithFemaleTotal = 0;
            double ageWithMaleTotal = 0;
            double ageWithFemaleTotal = 0;

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
                if (newItem.Gender == "M")
                {
                    maleCount++;
                    heightWithMaleTotal += newItem.Height;
                    weightWithMaleTotal += newItem.Weight;
                    ageWithMaleTotal += newItem.Age;
                }
                else
                {
                    femaleCount++;
                    heightWithFemaleTotal += newItem.Height;
                    weightWithFemaleTotal += newItem.Weight;
                    ageWithFemaleTotal += newItem.Age;
                }
            }
            double heightWithMaleMean = heightWithMaleTotal / maleCount;
            double heightWithMale_SD = 0;
            double heightWithFemaleMean = heightWithFemaleTotal / femaleCount;
            double heightWithFemale_SD = 0;

            double weightWithMaleMean = weightWithMaleTotal / maleCount;
            double weightWithMale_SD = 0;
            double weightWithFemaleMean = weightWithFemaleTotal / femaleCount;
            double weightWithFemale_SD = 0;

            double ageWithMaleMean = ageWithMaleTotal / maleCount;
            double ageWithMale_SD = 0;
            double ageWithFemaleMean = ageWithFemaleTotal / femaleCount;
            double ageWithFemale_SD = 0;

            foreach (var item in personsWithAge)
            {
                if (item.Gender == "M")
                {
                    heightWithMale_SD += Math.Pow(Math.Pow(item.Height - heightWithMaleMean, 2) / (maleCount - 1), 0.5); ;
                    weightWithMale_SD += Math.Pow(Math.Pow(item.Weight - weightWithMaleMean, 2) / (maleCount - 1), 0.5);
                    ageWithMale_SD += Math.Pow(Math.Pow(item.Age - ageWithMaleMean, 2) / (maleCount - 1), 0.5);
                }
                else
                {
                    heightWithFemale_SD += Math.Pow(Math.Pow(item.Height - heightWithFemaleMean, 2) / (femaleCount - 1), 0.5);
                    weightWithFemale_SD += Math.Pow(Math.Pow(item.Weight - weightWithFemaleMean, 2) / (femaleCount - 1), 0.5);
                    ageWithFemale_SD += Math.Pow(Math.Pow(item.Age - ageWithFemaleMean, 2) / (femaleCount - 1), 0.5);
                }
            }

            string[] testData1a2a = await File.ReadAllLinesAsync("testData1a2a.txt");
            foreach (var item in testData1a2a)
            {
                var itemDetail = item.Replace("(", "").Replace(")", "").Split(",");
                var newItem = new PersonWithAge()
                {
                    Height = double.Parse(itemDetail[0].Trim()),
                    Weight = double.Parse(itemDetail[1].Trim()),
                    Age = int.Parse(itemDetail[2].Trim())
                };
                double probabilityOfHeightGivenMale = (1 / (Math.Pow(2 * Math.PI, 0.5) * heightWithMale_SD)) * Math.Pow(Math.E, -Math.Pow(newItem.Height - heightWithMaleMean, 2) / Math.Pow(2 * heightWithMale_SD, 2));
                double probabilityOfWeightGivenMale = (1 / (Math.Pow(2 * Math.PI, 0.5) * weightWithMale_SD)) * Math.Pow(Math.E, -Math.Pow(newItem.Weight - weightWithMaleMean, 2) / Math.Pow(2 * weightWithMale_SD, 2));
                double probabilityOfAgeGivenMale = 1 / (Math.Pow(2 * Math.PI, 0.5) * ageWithMale_SD) * Math.Pow(Math.E, -Math.Pow(newItem.Age - ageWithMaleMean, 2) / Math.Pow(2 * ageWithMale_SD, 2));

                double probabilityOfHeightGivenFemale = (1 / (Math.Pow(2 * Math.PI, 0.5) * heightWithFemale_SD)) * Math.Pow(Math.E, -Math.Pow(newItem.Height - heightWithFemaleMean, 2) / Math.Pow(2 * heightWithFemale_SD, 2));
                double probabilityOfWeightGivenFemale = (1 / (Math.Pow(2 * Math.PI, 0.5) * weightWithFemale_SD)) * Math.Pow(Math.E, -Math.Pow(newItem.Weight - weightWithFemaleMean, 2) / Math.Pow(2 * weightWithFemale_SD, 2));
                double probabilityOfAgeGivenFemale = 1 / (Math.Pow(2 * Math.PI, 0.5) * ageWithFemale_SD) * Math.Pow(Math.E, -Math.Pow(newItem.Age - ageWithFemaleMean, 2) / Math.Pow(2 * ageWithFemale_SD, 2));

                double probabilityOfMale = maleCount / personsWithAge.Count;
                double probabilityOfFemale = femaleCount / personsWithAge.Count;


                double probabilityMaleTotal = probabilityOfHeightGivenMale + probabilityOfWeightGivenMale +
                    probabilityOfAgeGivenMale + probabilityOfMale;
                double probabilityFemaleTotal = probabilityOfHeightGivenFemale + probabilityOfWeightGivenFemale +
                    probabilityOfAgeGivenFemale + probabilityOfFemale;

                double finalTotalMale = probabilityMaleTotal / (probabilityMaleTotal + probabilityFemaleTotal);
                double finalTotalFemale = 1 - finalTotalMale;
                Console.WriteLine($"{finalTotalMale}//{finalTotalFemale}");

                if (finalTotalMale > finalTotalFemale)
                {
                    Console.WriteLine($"1a) Gender Prediction for Person with {newItem.Height}," +
                        $" {newItem.Weight} and {newItem.Age} using Gaussian Naive Bayes is M");
                }
                else
                {
                    Console.WriteLine($"1a) Gender Prediction for Person with {newItem.Height}," +
                        $" {newItem.Weight} and {newItem.Age} using Gaussian Naive Bayes is W");
                }
                Console.WriteLine("--------------------------------------");
                Console.WriteLine();
            }

        }
    }
}
