package com.bubbiewaschubbie;

import javax.swing.JTextField;

public class UserInput {
	public String template;
	
	public String name;
	public String weight;
	public String height_ft;
	public String height_in;
	public String sex;
	public String age;
	
	public void printDetails() {
		System.out.println("Template file: " + template);
		
		System.out.println("Name: " + name);
		System.out.println("Weight: " + weight);
		System.out.println("Height: " + height_ft + " " + height_in);
		System.out.println("Sex: " + sex);
		System.out.println("Age: " + age);
	
	}
	
	public void extractData(UserInterface ui) {
		template = ((JTextField)ui.template_list.get(0)).getText();
		
	}
}
