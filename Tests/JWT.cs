namespace Tests
{
    class JWT
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int[] Ids { get; set; }
    }
}
