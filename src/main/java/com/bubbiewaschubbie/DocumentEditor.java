package com.bubbiewaschubbie;

import java.io.FileNotFoundException;
import java.io.IOException;
import java.util.Map;

import com.itextpdf.forms.PdfAcroForm;
import com.itextpdf.forms.fields.PdfFormField;
import com.itextpdf.kernel.pdf.PdfDocument;
import com.itextpdf.kernel.pdf.PdfReader;
import com.itextpdf.kernel.pdf.PdfWriter;

public class DocumentEditor {
	private static String SOURCE_FILE = "template.pdf";
	private static String OUTPUT_FILE = "output.pdf";
	
	private PdfDocument pdfDoc;
	
	public DocumentEditor() {
		
	}
	
	public void setInputFile(String input) {
		SOURCE_FILE = input;
	}
	
	public void setOutputFile(String name) {
		OUTPUT_FILE = name + ".pdf";
	}
	
	public void openDocument() throws FileNotFoundException, IOException {
		pdfDoc = new PdfDocument(new PdfReader(SOURCE_FILE), new PdfWriter(OUTPUT_FILE));
    }
	
	public void saveDocument() {
        pdfDoc.close();
    }
	
	public void replaceText(String findText, String replaceText) {
		PdfAcroForm form = PdfAcroForm.getAcroForm(pdfDoc, true);
		Map<String, PdfFormField> fields = form.getFormFields();
		
		((PdfFormField) fields.get("name")).setValue("Hello");
	}

}
