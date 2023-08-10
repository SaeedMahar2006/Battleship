
namespace Battleship
{
    [Serializable]
    public class Matrix{
        //private Random rand = new Random();
        public int _n;
        public int _m;
        
        public dynamic matrix;

        public Matrix(int n, int m)
        {//N rows my M columns
            _n=n;
            _m=m;
            matrix = new double[n,m];
        }
        public double this[int x, int y]{
            get{
                return this.matrix[x,y];
            }
            set{
                this.matrix[x,y] = value;
            }
        }


        public void Print(){
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("   ");
            for(int i=0;i<this._n;i++) { Console.Write($"{i} ");}
            Console.WriteLine();
            for (int row=0; row<this._n;row++){
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{row}  ");
                for (int col=0; col<this._m;col++){
                    //string s=String.Format("{0,10:0.0}" , this.matrix[row,col]);
                    string s = (this.matrix[row,col]==-1)? "x": ((this.matrix[row, col] == 0)? "~": ((this.matrix[row,col]==0.5)? "~":"#"));
                    if(this.matrix[row,col]==0) {
                        Console.ForegroundColor = ConsoleColor.Cyan;                        
                    }
                    else if(this.matrix[row,col]<0){
                        Console.ForegroundColor = ConsoleColor.Red;
                    }else if (this.matrix[row, col] == 0.5)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }
                    else{
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    Console.Write($"{s} ");
                }Console.WriteLine();
            }Console.ResetColor();Console.WriteLine("\n\n");
        }

        public Matrix Transpose(){
            Matrix r = new Matrix(this._m,this._n); //note n m order switch, for non square matrix
            for (int row=0; row<this._n;row++){
                for (int col=0; col<this._m;col++){
                    r.matrix[col,row]=this.matrix[row,col];//note col row for new, row col for old
                }
            }
            return r;
        }

        public Matrix Copy_To(){
            Matrix r =new Matrix(this._n,this._m);
            for (int x = 0; x < this._m; x++)
            {
                for (int y = 0; y < this._n; y++)
                {
                    r.matrix[y,x]=this.matrix[y,x];
                }
            }
            return r;
        }
        public void Copy_To(Matrix r){
            if((r._n<this._n)|(r._m<this._m)) throw new Exception("Too small");
            for (int x = 0; x < this._m; x++)
            {
                for (int y = 0; y < this._n; y++)
                {
                    r.matrix[y,x]=this.matrix[y,x];
                }
            }
        }
        public Matrix Upscale(int rows, int columns){
            Matrix temp = new Matrix(this._n+rows,this._m+columns);
            this.Copy_To(temp);
            return temp;
        }
//add an upscale this so no need for new matrix? 

        public Matrix Minor(int x,int y){
            if((this._n!=1)&(this._m!=1)){
                Matrix r=new Matrix(this._n-1,this._m-1);
                //pull up and/or pull left idea
                for (int row=0;row<this._n;row++){
                    for(int col=0;col<this._m;col++){
                       switch ((row>y),(col>x)){
                        case (true,true):
                            //pull up and left
                            r.matrix[row-1,col-1]=this.matrix[row,col];
                            break;
                        case (true,false):
                            //pull up
                            r.matrix[row-1,col]=this.matrix[row,col];
                            break;
                        case (false,true):
                            //pull left
                            r.matrix[row,col-1]=this.matrix[row,col];
                            break;
                        case (false,false):
                            //nothing   or   vanish
                            if((row!=y)&(col!=x)){
                            r.matrix[row,col]=this.matrix[row,col];}
                            //if they are equal nothing happens
                            break;}

                    }
                }
                return r;
            }return this.matrix[0,0];
        }
        
        public static Matrix operator *(Matrix a, Matrix b){
            if (a._m==b._n){//dimension check   n m   n m     mid 2 should be same remember Further Math
                Matrix r = new Matrix(a._n,b._m);//dimensions of new matrix
                //loop through each cell and set r's element to sum of elements of a b
                for (int rrow=0;rrow<r._n;rrow++){
                    for(int rcol=0;rcol<r._m;rcol++){
                        //remember my working out
                        //rcol is b's col, rrow is a's row

                        //go through the a's row (acol++) and pair it with b elems
                        for (int acol=0;acol<a._m;acol++){
                            //note, since dimension check at start acol mathces for a and b.
                            r.matrix[rrow,rcol] += a.matrix[rrow, acol]*b.matrix[acol,rcol];
                            //look at diagram. += adds on stuffs

                        }//i think thats it

                    }
                }
                return r;
            }else{
                throw new Exception("Dimension error");
            }
        }




        public static Matrix operator +(Matrix a, Matrix b){
            if ((a._n==b._n)&(a._m==b._m)){
                Matrix r = new Matrix(a._n,a._m);
                //loop through each cell and set r's element to sum of elements of a b
                for (int row=0; row<a._n;row++){
                    for (int col=0; col<a._m;col++){
                        r.matrix[row,col]=a.matrix[row,col]+b.matrix[row,col];
                    } 
                }
                return r;
            }else{
                throw new Exception("Dimension error");
            }
        }

        public static Matrix operator *(double a, Matrix b){
            Matrix r = new Matrix(b._n,b._m);
            for(int x=0; x<b._n; x++){
                for (int y = 0; y < b._m; y++)
                {
                    r.matrix[x,y] = b.matrix[x,y]*a;
                }
            }
            return r;
        }
        public static Matrix operator /( Matrix b,double a){
            Matrix r = new Matrix(b._n,b._m);
            for(int x=0; x<b._n; x++){
                for (int y = 0; y < b._m; y++)
                {
                    r.matrix[x,y] = b.matrix[x,y]/a;
                }
            }
            return r;
        }



    }
}