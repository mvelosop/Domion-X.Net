Feature: (dflow-1) - Feature - Manage budget classes
	In order to manage my personal budget
	As the one responsible to do it
	I want to manage a list of general budget classes

Scenario: (dflow-1.1) - Scenario - Add budget classes
	
	Given the current user is working as tennant "dflow-1.1"

	Given there are no registered budget classes

	When I add the following budget classes:
		| Name                | TransactionType |
		| Income              | Income          |
		| Housing             | Expense         |
		| Transportation      | Expense         |
		| Insurance           | Expense         |
		| Food                | Expense         |
		| Children            | Expense         |
		| Legal               | Expense         |
		| Savings             | Savings         |
		| Investment          | Investment      |
		| Loans               | Loan            |
		| Entertainment       | Expense         |
		| Taxes               | Tax             |
		| Personal Care       | Expense         |
		| Pets                | Expense         |
		| Gifts and Donations | Expense         |

	Then I can find the following budget classes:
		| Name                | TransactionType |
		| Income              | Income          |
		| Housing             | Expense         |
		| Transportation      | Expense         |
		| Insurance           | Expense         |
		| Food                | Expense         |
		| Children            | Expense         |
		| Legal               | Expense         |
		| Savings             | Savings         |
		| Investment          | Investment      |
		| Loans               | Loan            |
		| Entertainment       | Expense         |
		| Taxes               | Tax             |
		| Personal Care       | Expense         |
		| Pets                | Expense         |
		| Gifts and Donations | Expense         |

