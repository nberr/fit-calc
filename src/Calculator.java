import java.awt.Component;

import javax.swing.*;


public class Calculator {
	
	public static void main(String[] args) {
		// initial frame
		JFrame f = new JFrame(); 
		f.setSize(450, 450);
		f.setLayout(null);
		
		// 4 panels to enter user info
		JPanel details  = new JPanel();
		JPanel goal     = new JPanel();
		JPanel activity = new JPanel();
		JPanel diet     = new JPanel();
		
		// defining the bounds of the tabs
		JTabbedPane tp = new JTabbedPane();
		tp.setBounds(25, 25, 400, 400);
		
		// adding the 4 panels to the tabbed pane
		tp.add("Details",  details);
		tp.add("Goal",     goal);
		tp.add("Activity", activity);
		tp.add("Diet",     diet);
		
		// adding the tabbed pane to the initial frame
		f.add(tp);
		
		f.setVisible(true);

	}

}
