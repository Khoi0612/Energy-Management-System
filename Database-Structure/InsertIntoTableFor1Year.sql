DO $$
DECLARE
    start_time TIMESTAMP WITHOUT TIME ZONE := '2025-01-01 00:00:00'; 
BEGIN
    WHILE CAST(start_time AS TIMESTAMP WITHOUT TIME ZONE) < '2025-12-01 00:00:00'::TIMESTAMP WITHOUT TIME ZONE LOOP
        INSERT INTO EnergyReadings (
            Start_Timestamp, End_Timestamp, I_L1, I_L2, I_L3, V_L1, V_L2, V_L3, V_L12, V_L23, V_L31,
            P_L1, P_L2, P_L3, VARQ1_L1, VARQ1_L2, VARQ1_L3, VA_L1, VA_L2, VA_L3,
            COS1_L1, COS1_L2, COS1_L3
        ) VALUES (
            start_time,
			start_time,
            ROUND(CAST(RANDOM() * 100 AS NUMERIC), 2),
            ROUND(CAST(RANDOM() * 100 AS NUMERIC), 2),
            ROUND(CAST(RANDOM() * 100 AS NUMERIC), 2),
            ROUND(CAST(RANDOM() * 300 AS NUMERIC), 2),
            ROUND(CAST(RANDOM() * 300 AS NUMERIC), 2),
            ROUND(CAST(RANDOM() * 300 AS NUMERIC), 2),
            ROUND(CAST(RANDOM() * 500 AS NUMERIC), 2),
            ROUND(CAST(RANDOM() * 500 AS NUMERIC), 2),
            ROUND(CAST(RANDOM() * 500 AS NUMERIC), 2),
            ROUND(CAST(RANDOM() * 50 AS NUMERIC), 2),
            ROUND(CAST(RANDOM() * 50 AS NUMERIC), 2),
            ROUND(CAST(RANDOM() * 50 AS NUMERIC), 2),
            ROUND(CAST(RANDOM() * 50 AS NUMERIC), 2),
            ROUND(CAST(RANDOM() * 50 AS NUMERIC), 2),
            ROUND(CAST(RANDOM() * 50 AS NUMERIC), 2),
            ROUND(CAST(RANDOM() * 50 AS NUMERIC), 2),
            ROUND(CAST(RANDOM() * 50 AS NUMERIC), 2),
            ROUND(CAST(RANDOM() * 50 AS NUMERIC), 2),
            ROUND(CAST(RANDOM() AS NUMERIC), 2),
            ROUND(CAST(RANDOM() AS NUMERIC), 2),
            ROUND(CAST(RANDOM() AS NUMERIC), 2)
        );
        SELECT start_time + interval '1 minute' INTO start_time;
    END LOOP;
END $$;