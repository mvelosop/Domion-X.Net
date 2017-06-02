Feature: (dflow-1) - Feature - Manage Budget Classes
	In order to manage my personal budget
	As the one responsible to do it
	I want to manage a list of general budget classes

Scenario: (dflow-1) - Scenario - Add new budget classes
	
	Given the following budget classes do not exist:
		| Name                 | TransactionType |
		| New - Income         | Income          |
		| New - Housing        | Expense         |
		| New - Transportation | Expense         |
		| New - Savings        | Savings         |
		| New - Investment     | Investment      |
		| New - Taxes          | Tax             |
		| New - Loans          | Loan            |

	When I add the following budget classes:
		| Name                 | TransactionType |
		| New - Income         | Income          |
		| New - Housing        | Expense         |
		| New - Transportation | Expense         |
		| New - Savings        | Savings         |
		| New - Investment     | Investment      |
		| New - Taxes          | Tax             |
		| New - Loans          | Loan            |

	Then I can find the following budget classes starting with "New - ":
		| Name                 | TransactionType |
		| New - Income         | Income          |
		| New - Housing        | Expense         |
		| New - Transportation | Expense         |
		| New - Savings        | Savings         |
		| New - Investment     | Investment      |
		| New - Taxes          | Tax             |
		| New - Loans          | Loan            |

