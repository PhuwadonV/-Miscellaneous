import java.util.regex.Pattern;
import java.util.*;

class Main {
	static int dim;
	static int num;
	static int[][] boxs;

	public static String inputStr() {
		return System.console().readLine();
	}
	
	public static int inputInt() {
		return Integer.parseInt(inputStr());
	}
	
	public static String boxString(int i) {
		String str = "" + boxs[i][0];
		for(int d = 1; d < dim; d++) str += "," + boxs[i][d];
		return str;
	}
	
	public static boolean insideable(int i, int j) {
		for(int d = 0; d < dim; d++) if(boxs[i][d] >= boxs[j][d]) return false;
		return true;
	}
	
	public static void inside(List<Integer> lst, String str, int i) {
		boolean last = true; 
		List<Integer> lst2 = new ArrayList<Integer>();
		for(int j = i + 1; j < num && !lst2.contains(j) && insideable(i, j); j++) {
			last = false;
			lst.add(j);
			if(j + 1 == num) System.out.println(str + " in " + boxString(j));
			else {
				inside(lst2, str + " in " + boxString(j),  j);
				lst.addAll(lst2);
			}
		}
		if(last) System.out.println(str);
	}
	
	public static void main(String[] args) {
		System.out.print("Box Dimension : ");
		dim = inputInt();
		
		System.out.print("Box Number : ");
		num = inputInt();
		
		boxs = new int[num][dim];
		
		for(int i = 0; i < num; i++) {
			System.out.print("Box " + (i + 1) + " : ");
			boxs[i] = Pattern.compile(",").splitAsStream(inputStr()).mapToInt(Integer::parseInt).toArray();
		}
		
		Arrays.<int[]>sort(boxs, (x, y) -> Integer.compare(x[0], y[0]));
		
		List<Integer> lst = new ArrayList<Integer>();
		
		for(int i = 0; i < num - 1; i++) if(!lst.contains(i)) inside(lst, boxString(i), i);
	}
}