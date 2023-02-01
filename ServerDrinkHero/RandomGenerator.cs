public class RandomGenerator {

    private static RandomGenerator _instance;
    private Random _rand;

    public static RandomGenerator Instance {
        get {
            if (_instance == null) {
                _instance = new RandomGenerator();
            }
            return _instance;
        }

    }
    public int NextRandom(int from, int to) {
        return _rand.Next(from, to);
    }


    public RandomGenerator() {
        _rand = new Random((int)DateTime.Now.Ticks);
    }
}

