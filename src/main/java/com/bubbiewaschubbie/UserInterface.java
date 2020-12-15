package com.bubbiewaschubbie;

import java.util.ArrayList;

import javax.swing.JButton;
import javax.swing.JComponent;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JTabbedPane;
import javax.swing.JTextField;

public class UserInterface {
	public JFrame frame;
	public JTabbedPane tp;
	public JButton generate;
	public UserInput ui;
	
	public ArrayList<JComponent> template_list;
	public ArrayList<JTextField> details_list;
	public ArrayList<JTextField> goal_list;
	public ArrayList<JTextField> activity_list;
	public ArrayList<JTextField> diet_list;
	
	public UserInterface() {
		frame = new JFrame("Fitness Calculator"); 
		frame.setSize(500, 500);
		frame.setLayout(null);
		
		tp = new JTabbedPane();
		tp.setBounds(10, 10, 480, 430);
		
		template_list = new ArrayList<JComponent>();
		inputInit();
		
		details_list = new ArrayList<JTextField>();
		detailsInit();
		
		goal_list = new ArrayList<JTextField>();
		goalInit();
		
		activity_list = new ArrayList<JTextField>();
		activityInit();
		
		diet_list = new ArrayList<JTextField>();
		dietInit();
		
		
		frame.add(tp);
		
		generate = new JButton("Generate Report");
		generate.setBounds(190, 445, 120, 20);
		
		
		frame.add(generate);
		
		frame.setResizable(false);
		frame.setVisible(true);
		frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
	
	}
	
	private void inputInit() {
		JPanel template = new JPanel();
		JLabel jl_template = new JLabel("Template file name");
		JTextField jtf_template = new JTextField(32);
		
		template_list.add(jtf_template);
		
		jtf_template.setText("template.pdf");
		
		jtf_template.setBounds(0, 0, 32, 32);
		
		template.add(jl_template);
		template.add(jtf_template);
		
		tp.add("File", template);
	}
	
	private void detailsInit() {
		JPanel details = new JPanel();
		
		tp.add("Details", details);
	}
	
	private void goalInit() {
		JPanel goal = new JPanel();
		
		tp.add("Goal", goal);
	}
	
	private void activityInit() {
		JPanel activity = new JPanel();
		
		tp.add("Activity", activity);
	}
	
	private void dietInit() {
		JPanel diet = new JPanel();
		
		tp.add("Diet", diet);
	}
}
