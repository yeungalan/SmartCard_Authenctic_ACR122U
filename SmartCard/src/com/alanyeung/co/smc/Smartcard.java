package com.alanyeung.co.smc;




import java.util.List;


import javax.smartcardio.Card;
import javax.smartcardio.CardChannel;
import javax.smartcardio.CardException;
import javax.smartcardio.CardTerminal;
import javax.smartcardio.CommandAPDU;
import javax.smartcardio.ResponseAPDU;
import javax.smartcardio.TerminalFactory;
import javax.xml.bind.DatatypeConverter;

public class Smartcard {

	public static void main(String[] args) throws CardException {
		// TODO Auto-generated method stub
		// Show the list of all available card readers:
		TerminalFactory factory = TerminalFactory.getDefault();
		List<CardTerminal> terminals = factory.terminals().list();
		System.out.println("Reader: " + terminals);
		// Use the first card reader:
		CardTerminal terminal = terminals.get(0);
		// Establish a connection with the card:
		Card card = terminal.connect("*");
		System.out.println("Card: " + card);
		CardChannel channel = card.getBasicChannel();
		
		byte[] data = hexStringToByteArray("0400000000"); 
		ResponseAPDU r = channel.transmit(new CommandAPDU(0xFF, 0x00, 0x40, 0x0F,data));
		String hex = DatatypeConverter.printHexBinary(r.getBytes());
		System.out.println("Response: " + hex);
		
	
		 data = hexStringToByteArray("D44A010100FFFF0000"); 
		 r = channel.transmit(new CommandAPDU(0xFF, 0x00, 0x00, 0x00,data));
		 hex = DatatypeConverter.printHexBinary(r.getBytes());
		 System.out.println("Response: " + hex);
		
		 data = hexStringToByteArray("0400000000"); 
		 r = channel.transmit(new CommandAPDU(0xFF, 0x00, 0x40, 0x0E,data));
		 hex = DatatypeConverter.printHexBinary(r.getBytes());
		 System.out.println("Response: " + hex);		
		
		// disconnect card:
		card.disconnect(false);
		
		System.out.println("End");	
	}
	
	public static byte[] hexStringToByteArray(String s) {
	    int len = s.length();
	    byte[] data = new byte[len / 2];
	    for (int i = 0; i < len; i += 2) {
	        data[i / 2] = (byte) ((Character.digit(s.charAt(i), 16) << 4)
	                             + Character.digit(s.charAt(i+1), 16));
	    }
	    return data;
	}

}
