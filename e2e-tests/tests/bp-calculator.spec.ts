import { test, expect } from '@playwright/test';

test.describe('BP Category Calculator', () => {

  // Reusable locators
  const locators = {
    heading: 'h4',
    form: '#form1',
    nameInput: 'input[name="BP.Name"]',
    systolicInput: 'input[name="BP.Systolic"]',
    diastolicInput: 'input[name="BP.Diastolic"]',
    submitButton: 'input[type="submit"]',
    validationSummary: '.text-danger',
    resultCategory: '.form-group .form-control[readonly]',
    BP_Systolic_error: `//div/input[@value='BP.Systolic']/following-sibling::span/span`,
    BP_Diastolic_error: `//div/input[@value='BP.Diastolic']/following-sibling::span/span`,
  };

  test('Home page loads and shows title', async ({ page }) => {
    await page.goto('/');
    await expect(page).toHaveTitle(/BP Category Calculator/i);
    await expect(page.locator(locators.heading)).toHaveText(/BP Category Calculator/i);
  });

  test('Calculates High Blood Pressure category', async ({ page }) => {
    await page.goto('/');

    // Fill the form
    await page.fill(locators.nameInput, 'Alice');
    await page.fill(locators.systolicInput, '150');
    await page.fill(locators.diastolicInput, '90');

    // Submit form
    await page.click(locators.submitButton);

    // Expect high BP category result
    const result = page.locator('text=High Blood Pressure');
    await expect(result).toBeVisible();
  });

  test('Shows validation error for invalid low values', async ({ page }) => {
    await page.goto('/');

    await page.fill(locators.nameInput, 'Bob');
    await page.fill(locators.systolicInput, '40'); 
    await page.fill(locators.diastolicInput, '20');
    await page.click(locators.submitButton);

    // Expect validation error message (adjust to match your app)
    const validationMessageSystolic = page.locator(locators.BP_Systolic_error);
    const text = await validationMessageSystolic.textContent();
    console.log("Validation text:", text);
    await expect(validationMessageSystolic).toContainText(/Invalid Systolic Value/i);

    const validationMessageDiastolic = page.locator(locators.BP_Diastolic_error);
    const text2 = await validationMessageDiastolic.textContent();
    console.log("Validation text:", text2);
    await expect(validationMessageDiastolic).toContainText(/Invalid Diastolic Value/i);
  });

});
