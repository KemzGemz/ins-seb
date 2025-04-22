@featureTest

Feature: Perform debit and credit transactions
  As a customer
  I want to perform debit and credit transaction
  So that I can manage my funds

  Scenario: Perform multiple transactions
    Given I am logged in as "Hermoine Granger"
    And I select account "1003"
    When I perform multiple transactions
    Then The current balance should tally with Balance section