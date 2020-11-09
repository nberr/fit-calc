import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;

import javax.swing.*;

import com.itextpdf.html2pdf.ConverterProperties;
import com.itextpdf.html2pdf.HtmlConverter;

public class Calculator {
	
	static String input_name;
	static String input_weight;
	static String input_height_ft;
	static String input_height_in;
	static String input_sex;
	static String input_age;
	
	private static void print_details() {
		System.out.println("Name: " + input_name);
		System.out.println("Weight: " + input_weight);
		System.out.println("Height: " + input_height_ft + " " + input_height_in);
		System.out.println("Sex: " + input_sex);
		System.out.println("Age: " + input_age);	
	}
	
	public static void main(String[] args) throws FileNotFoundException, IOException {
		// initial frame
		JFrame f = new JFrame("Fitness Calculator"); 
		f.setSize(500, 500);
		f.setLayout(null);
		
		// details tab
		JPanel details  = new JPanel();
		
		JLabel jl_name = new JLabel("Name");
		JTextField jtf_name = new JTextField(25);
		
		jl_name.setBounds(20, 20, 20, 20);
		jtf_name.setBounds(30, 30, 100, 20);
		
		details.add(jl_name);
		details.add(jtf_name);
		
		JLabel jl_weight = new JLabel("Weight");
		JTextField jtf_weight = new JTextField(10);
		
		jl_weight.setBounds(0, 0, 0, 0);
		jtf_weight.setBounds(0, 0, 0, 0);
		
		details.add(jl_weight);
		details.add(jtf_weight);
		
		JLabel jl_height = new JLabel("Height (ft in)");
		JTextField jtf_height_ft = new JTextField(5);
		JTextField jtf_height_in = new JTextField(5);
		
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
		
		// receive input into variables
		generate.addActionListener(new ActionListener() {
			@Override
			public void actionPerformed(ActionEvent arg0) {
				input_name = jtf_name.getText();
				input_weight = jtf_weight.getText();
				input_height_ft = jtf_height_ft.getText();
				input_height_in = jtf_height_in.getText();
				
				if (jrb_male.isSelected()) {
					input_sex = "Male";
				}
				else if (jrb_female.isSelected()) {
					input_sex = "Female";
				}
				else {
					input_sex = "Prefer not to say";
				}
				
				input_age = jtf_age.getText();
				
				print_details();
			}
		});
		
		// TODO: pull all info and add it to the html. then generate
		//       the pdf 
		
		File htmlSource = new File("res/pub/The Weight Loss Guide.html");
        File pdfDest = new File("output.pdf");
        
		ConverterProperties converterProperties = new ConverterProperties();
        HtmlConverter.convertToPdf(new FileInputStream(htmlSource), 
       new FileOutputStream(pdfDest), converterProperties);
		
		f.add(generate);
		
		f.setResizable(false);
		f.setVisible(true);
		f.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
	}

}
