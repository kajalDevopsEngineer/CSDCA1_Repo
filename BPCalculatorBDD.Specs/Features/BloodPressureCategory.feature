Feature: Blood pressure category calculation
  In order to understand my health
  As a user of the BP calculator
  I want my blood pressure reading to be classified correctly

  Scenario: High blood pressure when systolic is high
    Given my systolic is 150
    And my diastolic is 80
    When I calculate the blood pressure category
    Then the category should be "High Blood Pressure"

  Scenario: Ideal blood pressure in normal range
    Given my systolic is 110
    And my diastolic is 70
    When I calculate the blood pressure category
    Then the category should be "Ideal Blood Pressure"

  Scenario: Low blood pressure when values are low
    Given my systolic is 85
    And my diastolic is 55
    When I calculate the blood pressure category
    Then the category should be "Low Blood Pressure"
