using System.Collections.ObjectModel;
using System.Diagnostics;
using Xpense.Models;
using Xpense.Resources.Extensions;

namespace Xpense.ViewModels.DTOs
{
    public class Expense
    {
        public RecurringBill Bill { get; set; }
        public ObservableCollection<Cost> Costs { get; set; }

        public DateTime NextBillingDate => CalculateNextBillingDate();
        public DateTime FirstBillingDate => GetFirstBillingDate();
        public int DaysUntilBilled => CalculateDaysUntilBilled();
        public decimal AmountToBeBilled => GetMostRelevantPriceIndication()?.Amount ?? 0.00m;
        public string AmountToBeBilledToString => AmountToBeBilled.ToEuroString();
        public int PastBillingOccurrencedCount => GetBillingOccurrences();
        public decimal TotalBilledAmount => GetTotalBilledAmount();
        public string TotalBilledAmountToString => TotalBilledAmount.ToEuroString();

        public Expense(RecurringBill bill, ICollection<Cost> costs)
        {
            Bill = bill;
            Costs = new ObservableCollection<Cost>(costs.OrderByDescending(x => x.StartDate));
        }

        private Cost? GetMostRelevantPriceIndication()
        {
            // Find the most relevant price by considering both future and past costs
            var mostRelevantPrice = Costs
                .OrderByDescending(x => x.Created) // Sort by Created in descending order
                .FirstOrDefault();

            if (mostRelevantPrice == null)
            {
                Debug.WriteLine($"ERROR: an error occurred while getting the costs from bill `{Bill.Id}`, `{Bill.Name}`.");
            }

            return mostRelevantPrice;
        }

        /// <summary>
        /// Calculates the next billing date based on the most relevant price indication.
        /// </summary>
        /// <returns>The next billing date, or DateTime.MinValue if no relevant price indication found.</returns>
        private DateTime CalculateNextBillingDate()
        {
            var priceIndication = GetMostRelevantPriceIndication();
            return priceIndication?.NextBillingDate(Bill.RecurrenceInterval, Bill.RecurrenceIntervalMultiplier) ?? DateTime.MinValue;
        }

        /// <summary>
        /// Calculates the number of days until the next billing date.
        /// </summary>
        /// <returns>The number of days until the next billing date.</returns>
        private int CalculateDaysUntilBilled()
        {
            DateTime nextBillingDate = CalculateNextBillingDate();
            DateTime currentDate = DateTime.UtcNow;

            TimeSpan timeSpan = nextBillingDate.Date - currentDate.Date;
            return (int)timeSpan.TotalDays;
        }

        /// <summary>
        /// Gets the first billing date by finding the minimum start date among all costs.
        /// </summary>
        /// <returns>The first billing date, or DateTime.MinValue if no costs exist.</returns>
        private DateTime GetFirstBillingDate()
        {
            return Costs.Any() ? Costs.Min(x => x.StartDate) : DateTime.MinValue;
        }

        /// <summary>
        /// Gets the total number of billing occurrences for all costs.
        /// </summary>
        /// <returns>The total number of billing occurrences.</returns>
        public int GetBillingOccurrences()
        {
            int totalOccurrences = 0;
            foreach (var cost in Costs)
            {
                totalOccurrences += CalculateBillingOccurrences(cost);
            }
            return totalOccurrences;
        }

        /// <summary>
        /// Calculates the number of billing occurrences for a specific cost.
        /// </summary>
        /// <param name="cost">The cost for which to calculate billing occurrences.</param>
        /// <returns>The number of billing occurrences.</returns>
        private int CalculateBillingOccurrences(Cost cost)
        {
            var occurrences = 0;
            var billingDate = cost.StartDate;

            while (billingDate <= DateTime.UtcNow)
            {
                occurrences++;
                billingDate = billingDate.AddTimeInterval(Bill.RecurrenceInterval, Bill.RecurrenceIntervalMultiplier);
            }

            return occurrences - 1; // Subtract 1 because the last increment is in the future
        }

        /// <summary>
        /// Gets the total amount that has been billed so far.
        /// </summary>
        /// <returns>The total amount that has been billed.</returns>
        private decimal GetTotalBilledAmount()
        {
            decimal totalAmount = 0.00m;
            var currentDate = DateTime.UtcNow;

            for (int i = 0; i < Costs.Count; i++)
            {
                var currentCost = Costs[i];
                var nextCostStartDate = (i + 1 < Costs.Count) ? Costs[i + 1].StartDate : DateTime.MaxValue;
                var billingDate = currentCost.StartDate;

                while (billingDate < nextCostStartDate && billingDate <= currentDate)
                {
                    totalAmount += currentCost.Amount;
                    billingDate = billingDate.AddTimeInterval(Bill.RecurrenceInterval, Bill.RecurrenceIntervalMultiplier);
                }
            }

            return totalAmount;
        }
    }
}