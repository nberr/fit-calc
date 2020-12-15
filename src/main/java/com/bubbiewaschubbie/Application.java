package com.bubbiewaschubbie;

import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

public class Application {

	public static void main(String[] args) throws Exception {
		final DocumentEditor de = new DocumentEditor();
		final UserInterface ui = new UserInterface();
		//final Calculator c = new Calculator();
		final UserInput input = new UserInput();
		
		ui.generate.addActionListener(new ActionListener() {

			@Override
			public void actionPerformed(ActionEvent e) {
				// get the user input
				// this is handled by the UI generate report button
				// data must be extracted
				input.extractData(ui);
				
				// open the template file
				de.setInputFile(input.template);
				try {
					de.openDocument();
				} catch (Exception e1) {
					// TODO Auto-generated catch block
					e1.printStackTrace();
				}
				
				// make the calculations
				de.replaceText("<template>", "test");
				
				// write to the file
				de.saveDocument();
				
				// save the file
			}
			
		});
		
	}
}
