
# This is a FEATURE - think of it as "what are we testing?"
Feature: Employee Management
    As an API user
    I want to add employees to the system
    So that I can manage employee information


# This is our first SCENARIO - one specific test case
Scenario: Successfully add a new employee
    # GIVEN = Setup phase (what conditions exist before we test)
    Given I have the employee API available
    # AND = Additional setup (we can have multiple Given/When/Then statements)
    And I have valid employee data
    # WHEN = Action phase (what we're actually testing)
    When I send a POST request to add the employee
    # THEN = Verification phase (what should happen)
    Then the response should be successful
    # AND = Additional verification
    And the response should contain a success message