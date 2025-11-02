import pandas as pd
import numpy as np
from pathlib import Path
 
# === 1) Set your file paths here ===
CSV_PATHS = [
    "resources/dataset_project_eHealth20242025.csv",
    "resources/questionnaire_codebook_eHealth20242025.xlsx",
]
 
# (Optional) If you know the ID column name, set it here; otherwise the code will try to guess.
KNOWN_ID_COL = None  # e.g., "ID" or "RespondentID" or "participant_id"
 
# === 2) Helper: robust CSV loader (handles common encodings/delimiters) 
def load_csv_robust(path: str) -> pd.DataFrame:
    path = Path(path)
    if not path.exists():
        raise FileNotFoundError(f"File not found: {path}")
    # Try default; if it fails, try common alternatives
    trials = [
        dict(encoding=None, sep=None, engine="python"),          # auto-detect sep
        dict(encoding="utf-8", sep=None, engine="python"),
        dict(encoding="latin-1", sep=None, engine="python"),
        dict(encoding=None, sep=",", engine="python"),
        dict(encoding=None, sep=";", engine="python"),
    ]
    last_err = None
    for kw in trials:
        try:
            return pd.read_csv(path, **kw)
        except Exception as e:
            last_err = e
    raise RuntimeError(f"Could not read {path} – last error:\n{last_err}")
 
# === 3) Load both CSVs and show basic info ===
dfs = []
for p in CSV_PATHS:
    df = load_csv_robust(p)
    print(f"\n=== Loaded: {p} ===")
    print(f"Shape: {df.shape[0]} rows × {df.shape[1]} cols")
    print("First 10 column names:", list(df.columns[:10]))
    print("\nDtypes:")
    print(df.dtypes.head(15))
    dfs.append(df)
 
# === 4) Decide how to merge ===
# Case A: same schema -> just concatenate rows
def same_schema(df_list):
    if len(df_list) < 2:
        return True
    base_cols = set(df_list[0].columns)
    return all(set(d.columns) == base_cols for d in df_list[1:])
 
def guess_id_col(df: pd.DataFrame):
    candidates = [c for c in df.columns if c.lower() in ("id","respondentid","participant_id","participantid","subject_id","uid")]
    return candidates[0] if candidates else None
 
if same_schema(dfs):
    data = pd.concat(dfs, ignore_index=True)
    print("\n>> Merged by concatenation (same schema).")
else:
    # Case B: different schemas -> try to merge/align intelligently
    # 1) Try a known or guessed ID key for outer join
    key = KNOWN_ID_COL or guess_id_col(dfs[0]) or guess_id_col(dfs[1])
    if key and all(key in d.columns for d in dfs):
        data = dfs[0].merge(dfs[1], how="outer", on=key, suffixes=("_A","_B"))
        print(f"\n>> Merged by OUTER JOIN on ID column: '{key}'.")
    else:
        # 2) If no common key, align by column names (union of columns) and concatenate
        #    Missing columns are filled as NaN.
        all_cols = list(set().union(*[set(d.columns) for d in dfs]))
        aligned = [d.reindex(columns=all_cols) for d in dfs]
        data = pd.concat(aligned, ignore_index=True)
        print("\n>> Schemas differ and ID not found; concatenated with aligned columns (union).")
 
# === 5) Final quick check ===
print("\n=== FINAL DATASET ===")
print(f"Shape: {data.shape[0]} rows × {data.shape[1]} cols")
print("Sample columns:", list(data.columns[:20]))
print("\nDtypes (first 20):")
print(data.dtypes.head(20))
 
# Peek first rows (optional)
print("\nHead():")
print(data.head(3))