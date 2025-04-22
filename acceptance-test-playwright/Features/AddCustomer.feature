@featureTest

Feature: Add new customer
  As a bank manager
  I want to add a new customer
  So that they can open an account
  
  Scenario: Add, Validate and Delete customers as bank manager
    Given I am logged in as manager
    When I add all customers from the list
    Then all of them should appear in the customer table
    Then specific customers are deleted
    Then the deleted customers are no longer in the customer table