import java.awt.Component;

import javax.swing.*;


public class Calculator {
	
	public static void main(String[] args) {
		// initial frame
		JFrame f = new JFrame("Fitness Calculator"); 
		f.setSize(500, 500);
		f.setLayout(null);
		
		// details tab
		JPanel details  = new JPanel();
		
		JLabel jl_name = new JLabel("Name");
		JTextField jtf_name = new JTextField(25);
		String input_name = new String();
		
		jl_name.setBounds(20, 20, 20, 20);
		jtf_name.setBounds(30, 30, 100, 20);
		
		details.add(jl_name);
		details.add(jtf_name);
		
		JLabel jl_weight = new JLabel("Weight");
		JTextField jtf_weight = new JTextField(10);
		int input_weight;
		
		jl_weight.setBounds(0, 0, 0, 0);
		jtf_weight.setBounds(0, 0, 0, 0);
		
		details.add(jl_weight);
		details.add(jtf_weight);
		
		JLabel jl_height = new JLabel("Height (ft in)");
		JTextField jtf_height_ft = new JTextField(5);
		JTextField jtf_height_in = new JTextField(5);
		int input_height_ft;
		int input_height_in;
		
		jl_height.setBounds(0, 0, 0, 0);
		jtf_height_ft.setBounds(0, 0, 0, 0);
		jtf_height_in.setBounds(0,  0,  0,  0);
		
		details.add(jl_height);
		details.add(jtf_height_ft);
		details.add(jtf_height_in);
		
		JLabel jl_sex = new JLabel("Sex");
		ButtonGroup bg_sex = new ButtonGroup();
		JRadioButton jrb_male = new JRadioButton("Male");
		JRadioButton jrb_female = new JRadioButton("Female");
		String input_sex;
		
		jl_sex.setBounds(0, 0, 0, 0);
		jrb_male.setBounds(0, 0, 0, 0);
		jrb_female.setBounds(0, 0, 0, 0);
		
		bg_sex.add(jrb_male);
		bg_sex.add(jrb_female);
		
		details.add(jl_sex);
		details.add(jrb_male);
		details.add(jrb_female);
		
		JLabel jl_age = new JLabel("Age");
		JTextField jtf_age = new JTextField(5);
		int input_age;
		
		jl_age.setBounds(0, 0, 0, 0);
		jtf_age.setBounds(0, 0, 0, 0);
		
		details.add(jl_age);
		details.add(jtf_age);
		
		// goal tab
		JPanel goal     = new JPanel();
		
		// activity tab
		JPanel activity = new JPanel();
		
		// diet tab
		JPanel diet     = new JPanel();
		
		// defining the bounds of the tabs
		JTabbedPane tp = new JTabbedPane();
		tp.setBounds(10, 10, 480, 430);
		
		// adding the 4 panels to the tabbed pane
		tp.add("Details",  details);
		tp.add("Goal",     goal);
		tp.add("Activity", activity);
		tp.add("Diet",     diet);
		
		// adding the tabbed pane to the initial frame
		f.add(tp);
		
		JButton generate = new JButton("Generate Report");
		generate.setBounds(190, 445, 120, 20);
		
		// TODO: pull all info and add it to the html. then generate
		//       the pdf 
		
		f.add(generate);
		
		f.setResizable(false);
		f.setVisible(true);
		f.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
	}

}
