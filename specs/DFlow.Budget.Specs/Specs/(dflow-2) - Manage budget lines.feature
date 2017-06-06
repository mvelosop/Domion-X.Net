Feature: (dflow-2) - Feature - Manage budget lines
	In order to manage my personal budget
	As the one responsible to do it
	I want to manage a list of budget lines grouped in budget classes

Scenario: (dflow-2.1 - Scenario - Add budget lines

	Given the following budget classes exist:
		| Name           | TransactionType | Order |
		| Income         | Income          | 1     |
		| Housing        | Expense         | 2     |
		| Transportation | Expense         | 3     |
		| Insurance      | Expense         | 4     |
		| Food           | Expense         | 5     |
		| Children       | Expense         | 6     |
		| Savings        | Savings         | 7     |
		| Investment     | Investment      | 8     |
		| Loans          | Loan            | 9     |
		| Entertainment  | Expense         | 10    |
		| Taxes          | Tax             | 11    |
		| Personal Care  | Expense         | 12    |

	Then I can add the following buget lines:
		| 