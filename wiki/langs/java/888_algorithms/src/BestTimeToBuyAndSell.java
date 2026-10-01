public class BestTimeToBuyAndSell {
  public static void main(String[] args) {
    int[] prices = { 7, 1, 5, 3, 6, 4 };
    int maxProfit = maxProfit(prices);
    System.out.println("Maximum profit: " + maxProfit);
  }

  /**
   * Finds the maximum profit that can be achieved from a single buy and sell
   * operation given an array of stock prices.
   *
   * @param prices An array where the i-th element is the price of a given stock
   *               on
   *               day i.
   * @return The maximum profit that can be achieved. If no profit is possible,
   *         returns 0.
   */
  public static int maxProfit(int[] prices) {
    if (prices == null || prices.length == 0) {
      return 0; // No prices available
    }

    int minPrice = Integer.MAX_VALUE;
    int maxProfit = 0;

    for (int price : prices) {
      if (price < minPrice) {
        minPrice = price; // Update minimum price
      } else if (price - minPrice > maxProfit) {
        maxProfit = price - minPrice; // Update maximum profit
      }
    }
    return maxProfit;
  }
}
